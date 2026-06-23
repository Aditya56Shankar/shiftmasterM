using System;
using System.Linq;
using System.Text;
using AutoMapper;
using Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using NSwag;
using NSwag.Generation.Processors.Security;
using Services.Implementation;
using Services.Implementation;
using Services.Implementation.Repositories;
using Services.Implementation.Repositories.Services.Implementation;
using Services.Interfaces;
using Services.Interfaces.Repositories;
using Services.Mapper;
using ShiftMaster.Application.Implementation;
using NSwag.Generation.Processors.Security;
using Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. DATABASE CONNECTION CONFIGURATION
// =========================================================================
builder.Services.AddDbContext<ApplicationDbContext>(
    (sp, options) => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    contextLifetime: ServiceLifetime.Transient,
    optionsLifetime: ServiceLifetime.Singleton
);

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// =========================================================================
// 2. DOMAIN LOGIC SERVICES
// =========================================================================
builder.Services.AddScoped<IWorkLocationService, WorkLocationService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISkillRequirementService, SkillRequirementService>();
builder.Services.AddScoped<IShiftPatternService, ShiftPatternService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuditService, AuditService>();

// ✅ APPLICATION SERVICES (VERY IMPORTANT)
builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
builder.Services.AddScoped<ILeaveBlockService, LeaveBlockService>();
builder.Services.AddScoped<IEmployeeSkillService, EmployeeSkillService>();
builder.Services.AddScoped<IWeeklyRosterService, WeeklyRosterService>();


// =========================================================================
// 3. WORKFLOW ENGINE SERVICES
// =========================================================================
builder.Services.AddScoped<ICoverAssignmentService, CoverAssignmentService>();
builder.Services.AddScoped<IShiftSwapService, ShiftSwapService>();
builder.Services.AddScoped<IOvertimeService, OvertimeService>();
builder.Services.AddScoped<IRosterValidationService, RosterValidationService>();

// =========================================================================
// 4. REPOSITORIES
// =========================================================================

// Core repositories
builder.Services.AddScoped<ILeaveBlockRepository, LeaveBlockRepository>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
builder.Services.AddScoped<IEmployeeSkillRepository, EmployeeSkillRepository>();
builder.Services.AddScoped<IWeeklyRosterRepository, WeeklyRosterRepository>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<ICoverAssignmentRepository, CoverAssignmentRepository>();
builder.Services.AddScoped<IShiftSwapRepository, ShiftSwapRepository>();
builder.Services.AddScoped<IOvertimeRepository, OvertimeRepository>();

// New repositories added below
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ISkillRequirementRepository, SkillRequirementRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IShiftPatternRepository, ShiftPatternRepository>();
builder.Services.AddScoped<IWorkLocationRepository, WorkLocationRepository>();

// ✅ RosterValidation dependencies
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IViolationRepository, ViolationRepository>();
builder.Services.AddScoped<IStatusCheckRepository, StatusCheckRepository>();
// =========================================================================
// 5. CONTROLLERS, JSON, & AUTOMAPPER CONFIGURATION
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
// 6. JWT AUTHENTICATION & AUTHORIZATION CONFIGURATION
// =========================================================================
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

// =========================================================================
// 7. OPENAPI / NSWAG DOCUMENTATION CONFIGURATION
// =========================================================================
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "ShiftMaster API";
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
// 8. HTTP REQUEST PIPELINE (MIDDLEWARE)
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

// =========================================================================
// 9. DATABASE INITIALIZATION
// =========================================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.Run();