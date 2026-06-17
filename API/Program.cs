using AutoMapper;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Services.Implementation;
using Services.Interfaces;
using Services.Mapper;
using ShiftMaster.Application.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });


// ✅ Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ AutoMapper (Correct)

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

builder.Services.AddScoped<ILeaveBlockRepository, LeaveBlockRepository>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>(); 
builder.Services.AddScoped<IEmployeeSkillRepository, EmployeeSkillRepository>();
builder.Services.AddScoped<IWeeklyRosterRepository, WeeklyRosterRepository>();
builder.Services.AddScoped<IRosterValidationService, RosterValidationService>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();


// ✅ Controllers
builder.Services.AddControllers();

// ✅ OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// ✅ Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();