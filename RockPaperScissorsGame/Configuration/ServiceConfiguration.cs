using Microsoft.Extensions.DependencyInjection;
using RockPaperScissorsGame.Core.Interfaces;
using RockPaperScissorsGame.Core.UseCases;
using RockPaperScissorsGame.Infrastructure.Repositories;

namespace RockPaperScissorsGame.Configuration
{
    /// <summary>
    /// Configures services for the application.
    /// </summary>
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Add SignalR, Swagger, and controllers
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSignalR();

            // Register interactors
            services.AddScoped<CreateGameInteractor>();
            services.AddScoped<JoinGameInteractor>();
            services.AddScoped<MakeMoveInteractor>();
            services.AddScoped<GetGameStateInteractor>();

            // Register repositories
            services.AddSingleton<IGameRepository, InMemoryGameRepository>();

            // Add controllers
            services.AddControllers();
        }
    }
}
