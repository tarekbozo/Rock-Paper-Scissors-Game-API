using Microsoft.AspNetCore.Builder;
using RockPaperScissorsGame.Api.Middlewares;
using RockPaperScissorsGame.Infrastructure.SignalR;

namespace RockPaperScissorsGame.Configuration
{
    /// <summary>
    /// Configures middleware for the application.
    /// </summary>
    public static class MiddlewareConfiguration
    {
        public static void ConfigureMiddleware(WebApplication app)
        {
            // Enable Swagger and Developer Exception Page in Development environment
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                });
            }

            // Add custom middlewares
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RateLimitingMiddleware>();

            // Enable routing and map controllers/hubs
            app.UseRouting();
            app.UseAuthorization();

            // Map endpoints
            app.MapControllers();
            app.MapHub<GameHub>("/gamehub");
        }
    }
}
