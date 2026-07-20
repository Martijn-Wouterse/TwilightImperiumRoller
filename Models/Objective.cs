namespace TwilightImperiumRoller.Models;

public class Objective
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;
  public string Condition { get; set; } = string.Empty;
  public int Points { get; set; } = 1;
  public bool IsSecret { get; set; }
  public string Source { get; set; } = string.Empty;

  // Set when this objective was added directly to a single player (a secret objective), rather than to a round.
  public Guid? OwnerPlayerId { get; set; }
  public List<Guid> ScoredPlayerIds { get; set; } = [];
}
