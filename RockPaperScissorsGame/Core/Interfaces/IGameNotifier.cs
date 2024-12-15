using RockPaperScissorsGame.Core.Events;

namespace RockPaperScissorsGame.Core.Interfaces
{
    public interface IGameNotifier
    {
        Task NotifyGameUpdated(GameUpdatedEvent gameUpdatedEvent);
    }
}
