namespace TwilightImperiumRoller.Models;

/// <summary>A read-only catalog entry sourced from agendas.json, used to power the agenda name autocomplete.</summary>
public class AgendaCardDefinition
{
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string? Elect { get; set; }
  public string Effect { get; set; } = string.Empty;
  public string Source { get; set; } = string.Empty;
}
