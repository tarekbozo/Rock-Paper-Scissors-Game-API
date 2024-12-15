namespace RockPaperScissorsGame.Core.UseCases
{
    using RockPaperScissorsGame.Core.Entities;
    using RockPaperScissorsGame.Core.Exceptions;
    using RockPaperScissorsGame.Core.Interfaces;
    using RockPaperScissorsGame.Infrastructure.Logging;

    /// <summary>
    /// Handles the logic for a player joining an existing Rock-Paper-Scissors game.
    /// </summary>
    public class JoinGameInteractor
    {
        private readonly IGameRepository _gameRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinGameInteractor"/> class.
        /// </summary>
        /// <param name="gameRepository">The repository for accessing game data.</param>
        public JoinGameInteractor(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        /// <summary>
        /// Executes the logic for joining a game.
        /// </summary>
        /// <param name="gameId">The ID of the game to join.</param>
        /// <param name="playerName">The name of the player joining the game.</param>
        /// <returns>The updated game with the second player added.</returns>
        public Game Execute(Guid gameId, string playerName)
        {
            var game = _gameRepository.GetById(gameId)
                       ?? throw new GameNotFoundException($"Game with ID {gameId} not found");

            var error = game.Player2 != null ? "Game is already full" :
                (game.Player1?.Name == playerName || game.Player2?.Name == playerName) ?
                "A player with this name has already joined the game. Please use a unique name."
                 : null;

            if (error != null)
            {
                throw new InvalidOperationException(error);
           }


            game.Player2 = new Player { Name = playerName };
            game.Status = "Joined";

            _gameRepository.Save(game);
           

            return game;
        }
    }
}
