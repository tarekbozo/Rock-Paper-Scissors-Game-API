namespace RockPaperScissorsGame.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a player makes an invalid move.
    /// </summary>
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException(string message) : base(message) { }
    }

}
