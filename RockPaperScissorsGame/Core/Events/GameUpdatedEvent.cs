namespace RockPaperScissorsGame.Core.Events
{
    /// <summary>
    /// Represents an event triggered when a game is updated.
    /// </summary>
    public class GameUpdatedEvent
    {
        /// <summary>
        /// The unique identifier of the game.
        /// </summary>
        public Guid GameId { get; }

        /// <summary>
        /// The updated state of the game.
        /// </summary>
        public object GameState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameUpdatedEvent"/> class.
        /// </summary>
        /// <param name="gameId">The unique identifier of the game.</param>
        /// <param name="gameState">The updated state of the game.</param>
        public GameUpdatedEvent(Guid gameId, object gameState)
        {
            GameId = gameId;
            GameState = gameState;
        }
    }
}
