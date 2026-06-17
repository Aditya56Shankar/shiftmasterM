using System;
using System.Text;
using System.Linq;
using Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag; // Added to enable full NSwag layout controls
using NSwag.Generation.Processors.Security; // Enables security lock processors
using Services.Implementation;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Connection Configuration
// Register DbContext with options as singleton to allow factory (singleton) to consume them
builder.Services.AddDbContext<ApplicationDbContext>(
    (sp, options) => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    contextLifetime: ServiceLifetime.Transient,
    optionsLifetime: ServiceLifetime.Singleton);

// Also register the factory for AuditService (creates fresh contexts)
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Application Logic Services & Global JSON Configuration
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;

        //  THE FIX: Halts circular loops automatically across your entire API
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 3. JWT Authentication Configuration
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretKeyThatIsAtLeast32BytesLong!!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Prevent automatic inbound claim type mapping so the raw "role" claim is preserved
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ShiftMasterAPI",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ShiftMasterUsers",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            //  Maps the custom string "role" key directly to internal RBAC policies
            RoleClaimType = "role"
        };
    });

// 4. Role-Based Authorization Engine (Admin Policy Restriction)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminOnly", policy => policy.RequireRole("Admin"));
});

// 5. Fixed NSwag Document Generation
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "ShiftMaster API";
    document.Version = "v1";

    //  Switch from ApiKey to Http Bearer so NSwag handles the "Bearer " prefix prefixing automatically
    document.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Paste your raw token string below directly. (Do NOT type the word 'Bearer')."
    });

    // Use the ASP.NET Core specific processor so it reads your [Authorize] tags!
    document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

var app = builder.Build();

// 6. Request Middleware Pipeline Execution Map
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(); // Exposes the clean document schema file
    app.UseSwaggerUi(config =>
    {
        config.Path = "/swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication(); // 1. Read token claims to discover identity context and role values
app.UseAuthorization();  // 2. Enforce Role-Based validation checks (Validates Admin policy bounds)

app.MapControllers();

var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
db.Database.EnsureCreated(); // Auto-verifies schema health on boot

app.Run();