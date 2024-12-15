using RockPaperScissorsGame.Core.Entities;
using RockPaperScissorsGame.Core.Exceptions;
using RockPaperScissorsGame.Core.Interfaces;
using RockPaperScissorsGame.Core.Utilities;
using System.Threading.Tasks;

namespace RockPaperScissorsGame.Core.UseCases
{
    /// <summary>
    /// Handles the logic for making a move in a Rock-Paper-Scissors game.
    /// </summary>
    public class MakeMoveInteractor
    {
        private readonly IGameRepository _gameRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MakeMoveInteractor"/> class.
        /// </summary>
        /// <param name="gameRepository">The repository for accessing game data.</param>
        public MakeMoveInteractor(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
        }

        /// <summary>
        /// Executes the logic for making a move in the game.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <param name="playerName">The name of the player making the move.</param>
        /// <param name="move">The move being made.</param>
        /// <returns>The updated game state after the move.</returns>
        /// <exception cref="GameNotFoundException">Thrown when the game does not exist.</exception>
        /// <exception cref="UnauthorizedException">Thrown when the player is not part of the game.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the player has already made a move.</exception>
        public async Task<Game> Execute(Guid gameId, string playerName, string inputMove)
        {
            var move = MoveParser.Parse(inputMove); // Convert input to Move enum

            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new ArgumentException("Player name cannot be null or empty", nameof(playerName));
            }

            var game = _gameRepository.GetById(gameId)
                       ?? throw new GameNotFoundException($"Game with ID {gameId} not found");

            var player = GetPlayer(game, playerName)
                         ?? throw new UnauthorizedException($"Player {playerName} is not part of the game");

            if (player.CurrentMove.HasValue)
            {
                throw new InvalidOperationException($"Player {playerName} has already made a move");
            }

            player.CurrentMove = move;

            if (game.Player1?.CurrentMove.HasValue == true && game.Player2?.CurrentMove.HasValue == true)
            {
                DetermineWinner(game);
            }

            _gameRepository.Save(game);

            await Task.CompletedTask;

            return game;
        }

        /// <summary>
        /// Retrieves a player from the game by name.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="playerName">The name of the player.</param>
        /// <returns>The player instance if found, null otherwise.</returns>
        private static Player? GetPlayer(Game game, string playerName)
        {
            if (game.Player1?.Name?.Equals(playerName, StringComparison.OrdinalIgnoreCase) == true)
                return game.Player1;

            if (game.Player2?.Name?.Equals(playerName, StringComparison.OrdinalIgnoreCase) == true)
                return game.Player2;

            return null;
        }

        /// <summary>
        /// Determines the winner of the game based on player moves.
        /// </summary>
        /// <param name="game">The game whose winner is to be determined.</param>
        private static void DetermineWinner(Game game)
        {
            if (game.Player1?.CurrentMove == null || game.Player2?.CurrentMove == null)
            {
                throw new InvalidOperationException("Both players must have made a move to determine the winner.");
            }

            var move1 = game.Player1.CurrentMove.Value;
            var move2 = game.Player2.CurrentMove.Value;

            game.Winner = (move1, move2) switch
            {
                _ when move1 == move2 => "Tie",
                (Move.Rock, Move.Scissors) => game.Player1.Name,
                (Move.Scissors, Move.Paper) => game.Player1.Name,
                (Move.Paper, Move.Rock) => game.Player1.Name,
                _ => game.Player2.Name
            };

            game.Status = "Completed";
        }
    }
}
