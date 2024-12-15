namespace RockPaperScissorsGame.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a player attempts an unauthorized action.
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }

}
