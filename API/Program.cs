using Data.Context;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// 1. ADD SERVICES TO THE CONTAINER

// Register the Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// module 2 changes
builder.Services.AddScoped<IWorkLocationService, WorkLocationService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISkillRequirementService, SkillRequirementService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Register the Swagger/OpenAPI Generators (This fixes the InvalidOperationException!)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. CONFIGURE THE HTTP REQUEST PIPELINE

if (app.Environment.IsDevelopment())
{
    // Enable Swagger Middleware
    app.UseSwagger();

    // Enable the graphical Swagger UI Webpage
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShiftMaster API v1");
    });
}

// Automatically apply database migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseAuthorization();
app.MapControllers();

app.Run();