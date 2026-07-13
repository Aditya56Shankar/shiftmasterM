using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. REGISTER YARP REVERSE PROXY
// =========================================================================
// This reads the routes and clusters configurations from your appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// =========================================================================
// 2. REGISTER CORS POLICY FOR FRONTEND APP
// =========================================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("ShiftMasterFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Change to your frontend development port
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Enable CORS using the configured policy
app.UseCors("ShiftMasterFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// =========================================================================
// 3. SECURITY GATEWAY MIDDLEWARE: BLOCK/ALLOW INTERNAL INGRESS
// =========================================================================
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";

    // RULE A: If the request is internal service-to-service communication coming 
    // through the Gateway's "/gateway/*" prefix, let it pass directly to YARP.
    if (path.StartsWith("/gateway/", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    // RULE B: Block direct, unauthorized external access from users/ingress 
    // attempting to manually reach application endpoints starting with "/internal".
    if (context.Request.Path.StartsWithSegments("/internal", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsync("Not Found");
        return;
    }

    await next();
});

// =========================================================================
// 4. MAP YARP DOWNSTREAM PROXY ROUTING PIPELINES
// =========================================================================
app.MapReverseProxy();

app.Run();