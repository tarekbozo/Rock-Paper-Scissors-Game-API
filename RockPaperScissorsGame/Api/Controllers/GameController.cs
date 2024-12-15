using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RockPaperScissorsGame.Api.DTOs;
using RockPaperScissorsGame.Api.Presenters;
using RockPaperScissorsGame.Core.Interfaces;
using RockPaperScissorsGame.Core.UseCases;
using RockPaperScissorsGame.Infrastructure.SignalR;

namespace RockPaperScissorsGame.Api.Controllers
{
    /// <summary>
    /// Manages API endpoints for creating games, joining games, and making moves.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IHubContext<GameHub> _hubContext;
        private readonly GetGameStateInteractor _getGameStateInteractor;
        private readonly CreateGameInteractor _createGameInteractor;
        private readonly JoinGameInteractor _joinGameInteractor;
        private readonly MakeMoveInteractor _makeMoveInteractor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameController"/> class.
        /// </summary>
        /// <param name="hubContext">The SignalR hub context for broadcasting updates.</param>
        /// <param name="createGameInteractor">The interactor for creating games.</param>
        /// <param name="joinGameInteractor">The interactor for joining games.</param>
        /// <param name="makeMoveInteractor">The interactor for making moves in games.</param>
        public GameController(
            IHubContext<GameHub> hubContext,
            GetGameStateInteractor getGameStateInteractor,
            CreateGameInteractor createGameInteractor,
            JoinGameInteractor joinGameInteractor,
            MakeMoveInteractor makeMoveInteractor)
        {
            _hubContext = hubContext;
            _getGameStateInteractor = getGameStateInteractor;
            _createGameInteractor = createGameInteractor;
            _joinGameInteractor = joinGameInteractor;
            _makeMoveInteractor = makeMoveInteractor;
        }

        /// <summary>
        /// Creates a new Rock-Paper-Scissors game.
        /// </summary>
        /// <param name="playerDto">The details of the player creating the game.</param>
        /// <returns>The created game details.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateGame([FromBody] PlayerDTO playerDto)
        {

            if (string.IsNullOrWhiteSpace(playerDto.Name))
            {
                throw new ArgumentException("Player name cannot be null or empty", nameof(playerDto.Name));
            }
            var game = _createGameInteractor.Execute(playerDto.Name);

            // Notify clients about the new game
            await _hubContext.Clients.All.SendAsync("ReceiveGameUpdate", game.Id, GamePresenter.Present(game));

            return CreatedAtAction(nameof(CreateGame), GamePresenter.Present(game));
        }

        /// <summary>
        /// Allows a player to join an existing game.
        /// </summary>
        /// <param name="gameId">The ID of the game to join.</param>
        /// <param name="playerDto">The details of the player joining the game.</param>
        /// <returns>The updated game details.</returns>
        [HttpPost("{gameId}/join")]
        public async Task<IActionResult> JoinGame(Guid gameId, [FromBody] PlayerDTO playerDto)
        {
            if (string.IsNullOrWhiteSpace(playerDto.Name))
            {
                throw new ArgumentException("Player name cannot be null or empty", nameof(playerDto.Name));
            }
            var game = _joinGameInteractor.Execute(gameId, playerDto.Name);

            // Notify clients about the updated game
            await _hubContext.Clients.All.SendAsync("ReceiveGameUpdate", gameId, GamePresenter.Present(game));

            return Ok(GamePresenter.Present(game));
        }

        /// <summary>
        /// Allows a player to make a move in the game.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <param name="moveDto">The details of the player's move.</param>
        /// <returns>The updated game details.</returns>
        [HttpPost("{gameId}/move")]
        public async Task<IActionResult> MakeMove(Guid gameId, [FromBody] MoveDTO moveDto)
        {
            if (string.IsNullOrWhiteSpace(moveDto.Name))
            {
                throw new ArgumentException("Player name cannot be null or empty", nameof(moveDto.Name));
            }
            var game = await _makeMoveInteractor.Execute(gameId, moveDto.Name, moveDto.Move);

            // Notify clients about the game update
            await _hubContext.Clients.All.SendAsync("ReceiveGameUpdate", gameId, GamePresenter.Present(game));

            return Ok(GamePresenter.Present(game));
        }

        /// <summary>
        /// Retrieves the current state of the game with the specified ID.
        /// </summary>
        /// <param name="gameId">The ID of the game.</param>
        /// <returns>The current state of the game.</returns>
        [HttpGet("{gameId}")]
        public IActionResult GetGameState(Guid gameId)
        {
            var game = _getGameStateInteractor.Execute(gameId);

            if (game == null)
            {
                return NotFound(new { Message = $"Game with ID {gameId} not found." });
            }

            return Ok(GamePresenter.Present(game));
        }


    }
}
