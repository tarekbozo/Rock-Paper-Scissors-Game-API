namespace RockPaperScissorsGame.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a game cannot be found.
    /// </summary>
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(string message) : base(message) { }
    }

}
