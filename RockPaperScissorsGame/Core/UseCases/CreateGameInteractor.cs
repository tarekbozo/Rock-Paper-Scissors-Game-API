namespace RockPaperScissorsGame.Core.UseCases
{
    using RockPaperScissorsGame.Core.Entities;
    using RockPaperScissorsGame.Core.Interfaces;
    using RockPaperScissorsGame.Infrastructure.Logging;

    /// <summary>
    /// Handles the logic for creating a new Rock-Paper-Scissors game.
    /// </summary>
    public class CreateGameInteractor
    {
        private readonly IGameRepository _gameRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGameInteractor"/> class.
        /// </summary>
        /// <param name="gameRepository">The repository for persisting game data.</param>
        public CreateGameInteractor(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        /// <summary>
        /// Executes the creation of a new game.
        /// </summary>
        /// <param name="playerName">The name of the player initiating the game.</param>
        /// <returns>The newly created game.</returns>
        public Game Execute(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new ArgumentException("Player name cannot be null or empty", nameof(playerName));
            }

            var player = new Player { Name = playerName };

            var game = new Game
            {
                Id = Guid.NewGuid(),
                Player1 = player,
                Status = "Created"
            };

            _gameRepository.Save(game);
            Logger.LogInformation($"Game successfully created with ID: {game.Id}, initiated by Player: {playerName}");

            return game;
        }
    }
}
