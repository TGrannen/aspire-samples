﻿using AspireWithSerilog.Web;
using AspireWithSerilog.Web.Components;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Log.Logger.ConfigureSerilogBootstrapLogger();
// Add service defaults & Aspire components.

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new("http://apiservice"));

var app = builder.Build();
app.UseSerilogRequestLogging();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();

app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();



try
{
    Log.Information("Starting the Web Frontend!"); // Logs with the boostrap logger
    app.Run();
}
catch (Exception ex)
{
    // Logs with the boostrap logger if an exception is thrown during start up
    Log.Fatal(ex, "Web Frontend terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
