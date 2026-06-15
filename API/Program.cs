using AutoMapper;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Services.Implementation;
using Services.Interfaces;
using Services.Mapper;

var builder = WebApplication.CreateBuilder(args);

// ✅ DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ AutoMapper (v16 correct way)
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// ✅ Repository
builder.Services.AddScoped<IWeeklyRosterRepository, WeeklyRosterRepository>();

// ✅ Controllers
builder.Services.AddControllers();
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