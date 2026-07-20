namespace TwilightImperiumRoller.Models;

public class GameRound
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public int RoundNumber { get; set; }
  public List<Guid> ObjectiveIds { get; set; } = [];
}
