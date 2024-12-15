using RockPaperScissorsGame.Api.DTOs;
using RockPaperScissorsGame.Core.Entities;

namespace RockPaperScissorsGame.Api.Presenters
{
    public static class GamePresenter
    {
        /// <summary>
        /// Converts a Game domain entity to a GameResponseDTO for API responses.
        /// </summary>
        /// <param name="game">The Game entity to be presented.</param>
        /// <returns>A formatted GameResponseDTO object.</returns>
        public static GameResponseDTO Present(Game game)
        {
            return new GameResponseDTO
            {
                Id = game.Id.ToString(),
                Status = game.Status,
                Player1 = game.Player1 == null
                    ? null
                    : new PlayerDetailsDTO
                    {
                        Name = game.Player1.Name,
                        Move = game.Player1?.CurrentMove?.ToString()
                    },
                Player2 = game.Player2 == null
                    ? null
                    : new PlayerDetailsDTO
                    {
                        Name = game.Player2.Name,
                        Move = game.Player2?.CurrentMove?.ToString()
                    },
                Winner = game.Winner
            };
        }
    }
}
