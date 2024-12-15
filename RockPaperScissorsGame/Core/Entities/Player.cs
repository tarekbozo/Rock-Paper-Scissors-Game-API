namespace RockPaperScissorsGame.Core.Entities
{
    /// <summary>
    /// Represents a player in the Rock-Paper-Scissors game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The name of the player.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The player's current move in the game. Null if the move has not yet been made.
        /// </summary>
        public Move? CurrentMove { get; set; }
    }

}
