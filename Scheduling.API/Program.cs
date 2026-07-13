using System;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using ShiftMaster.SchedulingService.Clients;
using ShiftMaster.SchedulingService.Application.Services;
using ShiftMaster.SchedulingService.Application.Interfaces;
using ShiftMaster.SchedulingService.Infrastructure.Repositories;
using ShiftMaster.SchedulingService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. DATABASE CONNECTION CONFIGURATION
// =========================================================================
builder.Services.AddDbContext<SchedulingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// =========================================================================
// 2. REPOSITORIES
// =========================================================================
builder.Services.AddScoped<IWeeklyRosterRepository, WeeklyRosterRepository>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IShiftSwapRepository, ShiftSwapRepository>();
builder.Services.AddScoped<ICoverAssignmentRepository, CoverAssignmentRepository>();
builder.Services.AddScoped<IStatusCheckRepository, StatusCheckRepository>();
builder.Services.AddScoped<IShiftPatternRepository, ShiftPatternRepository>();
builder.Services.AddScoped<IViolationRepository, ViolationRepository>();

// =========================================================================
// 3. HTTP CLIENTS & CONTEXT TOKEN FORWARDING HANDLERS
// =========================================================================
// Required for extracting the bearer token out of the incoming supervisor request context
// =========================================================================
// 3. HTTP CLIENTS & SPECIAL CLIENTS
// =========================================================================
// =========================================================================
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenForwardingHandler>(); // Register the handler

builder.Services.AddHttpClient<IIdentityClient, IdentityClient>(client =>
{
    var baseUrl = builder.Configuration["Services:IdentityService"] ?? "https://localhost:64101/";
    client.BaseAddress = new Uri(baseUrl);
})
.AddHttpMessageHandler<TokenForwardingHandler>(); // Attach it here cleanly!

builder.Services.AddHttpClient<IEmployeeClient, EmployeeClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:EmployeeService"] ?? "https://localhost:64106/");
});

builder.Services.AddHttpClient<IAuditService, HttpAuditService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:CommsAuditService"] ?? "https://localhost:64099/");
});
// =========================================================================
// 4. SERVICES
// =========================================================================
builder.Services.AddScoped<IWeeklyRosterService, WeeklyRosterService>();
builder.Services.AddScoped<IRosterValidationService, RosterValidationService>();
builder.Services.AddScoped<IShiftPatternService, ShiftPatternService>();
builder.Services.AddScoped<IShiftSwapService, ShiftSwapService>();
builder.Services.AddScoped<ICoverAssignmentService, CoverAssignmentService>();

// =========================================================================
// 5. CONTROLLERS, JSON & AUTOMAPPER CONFIGURATION
// =========================================================================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

// =========================================================================
// 6. JWT AUTHENTICATION
// =========================================================================
var jwtKey = builder.Configuration["Jwt:Key"] ?? "superSecretKeyShiftMaster123!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = "role"
        };
    });

// =========================================================================
// 7. OPENAPI / NSWAG DOCUMENTATION
// =========================================================================
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "ShiftMaster Scheduling Service API";
    document.Version = "v1";

    document.AddSecurity("Bearer", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter: Bearer {your JWT token}"
    });

    document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

// =========================================================================
// 8. MIDDLEWARE PIPELINE ORCHESTRATION
// =========================================================================
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure DB is initialized
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchedulingDbContext>();
    db.Database.EnsureCreated();
}

app.Run();