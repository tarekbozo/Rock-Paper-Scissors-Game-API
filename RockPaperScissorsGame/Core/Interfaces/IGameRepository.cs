using RockPaperScissorsGame.Core.Entities;

namespace RockPaperScissorsGame.Core.Interfaces
{
    /// <summary>
    /// Defines methods for interacting with game data storage.
    /// </summary>
    public interface IGameRepository
    {
        /// <summary>
        /// Persists the game to storage.
        /// </summary>
        /// <param name="game">The game to save.</param>
        void Save(Game game);

        /// <summary>
        /// Retrieves a game by its ID.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <returns>The game with the specified ID, or null if not found.</returns>
        Game GetById(Guid gameId);
    }

}
