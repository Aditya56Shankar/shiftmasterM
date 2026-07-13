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
using ShiftMaster.AttendanceService.Clients;
using ShiftMaster.AttendanceService.Applications.Services;
using ShiftMaster.AttendanceService.Applications.Interfaces;
using ShiftMaster.AttendanceService.Infrastructure.Data;
using ShiftMaster.AttendanceService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. DATABASE CONNECTION CONFIGURATION
builder.Services.AddDbContext<AttendanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 2. REPOSITORIES
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IOvertimeRepository, OvertimeRepository>();

// =========================================================================
// 3. HTTP CLIENTS & SPECIAL CLIENTS (WITH TOKEN FORWARDING HANDLERS)
// =========================================================================
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenForwardingHandler>(); // Intercepts and forwards the JWT securely

builder.Services.AddHttpClient<IIdentityClient, IdentityClient>(client =>
{
    // Identity Service running on port 64101
    client.BaseAddress = new Uri(builder.Configuration["Services:IdentityService"] ?? "https://localhost:64101/");
})
.AddHttpMessageHandler<TokenForwardingHandler>();

builder.Services.AddHttpClient<ISchedulingClient, SchedulingClient>(client =>
{
    // Scheduling Service running on port 64105
    client.BaseAddress = new Uri(builder.Configuration["Services:SchedulingService"] ?? "https://localhost:64105/");
})
.AddHttpMessageHandler<TokenForwardingHandler>();

builder.Services.AddHttpClient<IEmployeeClient, EmployeeClient>(client =>
{
    // Employee Service running on port 64106
    client.BaseAddress = new Uri(builder.Configuration["Services:EmployeeService"] ?? "https://localhost:64106/");
})
.AddHttpMessageHandler<TokenForwardingHandler>();

builder.Services.AddHttpClient<IAuditService, HttpAuditService>(client =>
{
    // Comms Audit Service running on port 64099
    client.BaseAddress = new Uri(builder.Configuration["Services:CommsAuditService"] ?? "https://localhost:64099/");
})
.AddHttpMessageHandler<TokenForwardingHandler>();

// 4. SERVICES
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IOvertimeService, OvertimeService>();

// 5. CONTROLLERS & SERIALIZATION & MAPPER
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
// 6. JWT AUTHENTICATION
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

// 7. OPENAPI / NSWAG DOCUMENTATION
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "ShiftMaster Attendance Service API";
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
    var db = scope.ServiceProvider.GetRequiredService<AttendanceDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
