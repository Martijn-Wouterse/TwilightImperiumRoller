using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public interface IGameStateRepository
{
  Task<GameStateModel?> LoadAsync();
  Task SaveAsync(GameStateModel model);
}
