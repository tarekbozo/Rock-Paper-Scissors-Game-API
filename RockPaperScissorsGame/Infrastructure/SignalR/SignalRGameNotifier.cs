using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RockPaperScissorsGame.Core.Events;
using RockPaperScissorsGame.Core.Interfaces;

namespace RockPaperScissorsGame.Infrastructure.SignalR
{
    /// <summary>
    /// Implements the IGameNotifier interface to send game updates using SignalR.
    /// </summary>
    public class SignalRGameNotifier : IGameNotifier
    {
        private readonly IHubContext<GameHub> _hubContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRGameNotifier"/> class.
        /// </summary>
        /// <param name="hubContext">The SignalR hub context used to send updates to clients.</param>
        public SignalRGameNotifier(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Sends a notification to all SignalR clients about a game update.
        /// </summary>
        /// <param name="gameUpdatedEvent">The event containing the game ID and updated state.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task NotifyGameUpdated(GameUpdatedEvent gameUpdatedEvent)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveGameUpdate", gameUpdatedEvent.GameId, gameUpdatedEvent.GameState);
        }
    }
}
