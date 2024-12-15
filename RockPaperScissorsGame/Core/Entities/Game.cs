
namespace RockPaperScissorsGame.Core.Entities
{
    /// <summary>
    /// Represents a game of Rock-Paper-Scissors.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Unique identifier for the game.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// The first player in the game.
        /// </summary>
        public Player? Player1 { get; set; }

        /// <summary>
        /// The second player in the game.
        /// </summary>
        public Player? Player2 { get; set; }

        /// <summary>
        /// The current status of the game. Possible values: Created, Joined, Completed.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// The winner of the game. Null until the game is completed.
        /// </summary>
        public string? Winner { get; set; }
    }

}
