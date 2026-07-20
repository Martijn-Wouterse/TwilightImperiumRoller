using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public interface IDiceRollingService
{
  int RollDie();
  int CountHits(IEnumerable<ShipStats> shipStats);
  TimeSpan GetRollDuration(int totalDiceInPool);
  int GetTickDelayMilliseconds(TimeSpan elapsed, TimeSpan totalDuration);
}
