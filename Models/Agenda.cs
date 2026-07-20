namespace TwilightImperiumRoller.Models;

public class Agenda
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;
  public string? Elect { get; set; }
  public string Effect { get; set; } = string.Empty;
}
