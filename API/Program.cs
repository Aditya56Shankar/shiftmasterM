using System;
using System.Linq;
using System.Text;
using AutoMapper;
using Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi; // Native OpenAPI namespace
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using NSwag;
     // Native OpenApi Object models
using Services.Implementation;
using Services.Implementation.Repositories;
using Services.Interfaces;
using Services.Interfaces.Repositories;
using Services.Mapper;
using ShiftMaster.Application.Implementation;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(
    (sp, options) => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    contextLifetime: ServiceLifetime.Transient,
    optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IWorkLocationService, WorkLocationService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISkillRequirementService, SkillRequirementService>();
builder.Services.AddScoped<IShiftPatternService, ShiftPatternService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuditService, AuditService>();

// Workflow Engine Services
builder.Services.AddScoped<ICoverAssignmentService, CoverAssignmentService>();
builder.Services.AddScoped<IShiftSwapService, ShiftSwapService>();
builder.Services.AddScoped<IOvertimeService, OvertimeService>();

// Repository Dependency Injection
builder.Services.AddScoped<ILeaveBlockRepository, LeaveBlockRepository>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
builder.Services.AddScoped<IEmployeeSkillRepository, EmployeeSkillRepository>();
builder.Services.AddScoped<IWeeklyRosterRepository, WeeklyRosterRepository>();
builder.Services.AddScoped<IRosterValidationService, RosterValidationService>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<ICoverAssignmentRepository, CoverAssignmentRepository>();
builder.Services.AddScoped<IShiftSwapRepository, ShiftSwapRepository>();
builder.Services.AddScoped<IOvertimeRepository, OvertimeRepository>();

// Controllers & JSON Setup (Consolidated into single declaration)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// AutoMapper (Consolidated)
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});


var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretKeyThatIsAtLeast32BytesLong!!";
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
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ShiftMasterAPI",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ShiftMasterUsers",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = "role"
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminOnly", policy => policy.RequireRole("Admin"));
});


// Removed the conflicting builder.Services.AddOpenApi() wrapper
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "ShiftMaster API";
    document.Version = "v1";

    // Explicitly declaring 'NSwag.' prevents the ambiguous reference error
    document.AddSecurity("Bearer", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Paste your raw token string below directly. (Do NOT type the word 'Bearer')."
    });

    // This processor reads your [Authorize] attributes to lock down the endpoints
    document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Use NSwag middleware to serve the JSON and the UI (which contains the auth button)
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

// Strict Execution Order: Auth MUST execute before mapping controllers
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Database initialization block
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.Run();