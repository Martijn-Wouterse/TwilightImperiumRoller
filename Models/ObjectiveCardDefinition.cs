namespace TwilightImperiumRoller.Models;

/// <summary>A read-only catalog entry sourced from objectives.json, used to power the objective name autocomplete.</summary>
public class ObjectiveCardDefinition
{
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string Condition { get; set; } = string.Empty;
  public bool IsSecret { get; set; }
  public int Points { get; set; }
  public string Source { get; set; } = string.Empty;
}
