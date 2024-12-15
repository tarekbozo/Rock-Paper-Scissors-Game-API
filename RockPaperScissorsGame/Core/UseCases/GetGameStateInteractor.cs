using RockPaperScissorsGame.Core.Entities;
using RockPaperScissorsGame.Core.Exceptions;
using RockPaperScissorsGame.Core.Interfaces;

namespace RockPaperScissorsGame.Core.UseCases
{
    public class GetGameStateInteractor
    {
        private readonly IGameRepository _gameRepository;

        public GetGameStateInteractor(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Game Execute(Guid gameId)
        {
            return _gameRepository.GetById(gameId)
                ?? throw new GameNotFoundException($"Game with ID {gameId} not found.");
        }
    }

}
