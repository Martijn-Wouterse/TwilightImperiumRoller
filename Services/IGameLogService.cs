namespace TwilightImperiumRoller.Services;

public interface IGameLogService
{
  /// <summary>Appends a timestamped, human-readable line to the game log file.</summary>
  Task AppendAsync(string message);
}
