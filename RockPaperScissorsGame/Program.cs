using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using RockPaperScissorsGame.Configuration;
using RockPaperScissorsGame.Infrastructure.Logging;
using Serilog;

try
{
    // Configure Serilog for logging
    Logger.ConfigureLogging();
    Log.Information("Starting the application...");

    // Create and configure the builder
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container
    ServiceConfiguration.ConfigureServices(builder.Services);

    // Build the application
    var app = builder.Build();

    // Configure the HTTP request pipeline
    MiddlewareConfiguration.ConfigureMiddleware(app);

    // Run the application
    app.Run();
}
catch (Exception ex)
{
    // Log any fatal startup errors
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    // Ensure proper shutdown and log flushing
    Log.CloseAndFlush();
}
