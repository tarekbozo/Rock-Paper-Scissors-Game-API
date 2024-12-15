using RockPaperScissorsGame.Core.Entities;
using RockPaperScissorsGame.Core.Exceptions;
using RockPaperScissorsGame.Core.Interfaces;
using RockPaperScissorsGame.Infrastructure.Logging;
using System.Collections.Concurrent;

namespace RockPaperScissorsGame.Infrastructure.Repositories
{
    /// <summary>
    /// In-memory implementation of the game repository.
    /// </summary>
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly ConcurrentDictionary<Guid, Game> _games = new();


        /// <summary>
        /// Persists the game to storage.
        /// </summary>
        /// <param name="game">The game to save.</param>
        public void Save(Game game)
        {
            // Ensure the game object is not null
            if (game == null)
            {
                Logger.LogError("Cannot save a null game.");
                throw new ArgumentNullException(nameof(game), "Game cannot be null.");
            }

            // Attempt to add the game to the dictionary
            if (_games.TryAdd(game.Id, game))
            {
                Logger.LogInformation("Game successfully saved with ID: {GameId}", game.Id);
            }
            else
            {
                // If the game already exists, update it instead
                _games[game.Id] = game;
                Logger.LogInformation("Game with ID: {GameId} already exists. Overwriting existing game.", game.Id);
            }

            // Log the current state of the repository for debugging
            Logger.LogInformation("Current games in repository: {@GameIds}", _games.Keys);
        }

        /// <summary>
        /// Retrieves a game by its ID.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <returns>The game with the specified ID, or null if not found.</returns>
        public Game GetById(Guid gameId)
        {
            var game = _games.TryGetValue(gameId, out var foundGame) ? foundGame : null;
            if (game == null)
            {
                Logger.LogError($"Game not found with ID: {gameId}");
                throw new GameNotFoundException($"Game with ID {gameId} not found");
            }
            return game; 
        }

    }
}
