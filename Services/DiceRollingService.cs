using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public class DiceRollingService : IDiceRollingService
{
  private const int DieSides = 10;
  private const int MinRollDurationMs = 1000;
  private const int MaxRollDurationSeconds = 4;
  private const int BaseTickDelayMs = 40;
  private const int MaxAdditionalTickDelayMs = 200;

  public int RollDie() => Random.Shared.Next(1, DieSides + 1);

  public int CountHits(IEnumerable<ShipStats> shipStats) =>
    shipStats.Sum(ship => ship.Results.Count(result => result.FinalValue != null && result.FinalValue >= ship.Hit));

  public TimeSpan GetRollDuration(int totalDiceInPool)
  {
    int maxSeconds = totalDiceInPool <= MaxRollDurationSeconds ? totalDiceInPool : MaxRollDurationSeconds;
    int durationMs = Random.Shared.Next(MinRollDurationMs, maxSeconds * 1000);
    return TimeSpan.FromMilliseconds(durationMs);
  }

  public int GetTickDelayMilliseconds(TimeSpan elapsed, TimeSpan totalDuration)
  {
    double progress = elapsed.TotalMilliseconds / totalDuration.TotalMilliseconds;
    double eased = progress * progress;
    return BaseTickDelayMs + (int)(eased * MaxAdditionalTickDelayMs);
  }
}
