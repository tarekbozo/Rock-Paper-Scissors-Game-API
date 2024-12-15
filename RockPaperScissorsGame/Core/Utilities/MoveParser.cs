using RockPaperScissorsGame.Core.Entities;

namespace RockPaperScissorsGame.Core.Utilities
{
    public static class MoveParser
    {
        public static Move Parse(string input)
        {

            if (Enum.TryParse(typeof(Move), input, true, out var parsedMove))
                return (Move)parsedMove;

            throw new ArgumentException("Invalid move. Must be 'Rock', 'Paper', 'Scissors'", nameof(input));
        }
    }

}
