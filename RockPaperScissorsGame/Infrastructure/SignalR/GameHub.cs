using Microsoft.AspNetCore.SignalR;

namespace RockPaperScissorsGame.Infrastructure.SignalR
{
    public class GameHub : Hub
    {
        public async Task JoinGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Group(gameId).SendAsync("PlayerJoined", $"{Context.ConnectionId} has joined the game {gameId}");
        }

        public async Task NotifyMove(string gameId, string playerName, string move)
        {
            await Clients.Group(gameId).SendAsync("MoveMade", playerName, move);
        }

        public async Task NotifyGameEnd(string gameId, string winner)
        {
            await Clients.Group(gameId).SendAsync("GameEnded", $"The winner is {winner}");
        }
    }
}
