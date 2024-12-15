
using RockPaperScissorsGame.Core.Interfaces;
using RockPaperScissorsGame.Core.UseCases;
using RockPaperScissorsGame.Infrastructure.Repositories;

namespace RockPaperScissorsAPI.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IGameRepository, InMemoryGameRepository>();
            services.AddScoped<CreateGameInteractor>();
            services.AddScoped<JoinGameInteractor>();
            services.AddScoped<MakeMoveInteractor>();
            return services;
        }
    }
}
