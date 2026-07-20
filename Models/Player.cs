namespace TwilightImperiumRoller.Models;

public class Player
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;
  public Faction Faction { get; set; }
  public string Color { get; set; } = string.Empty;
}
