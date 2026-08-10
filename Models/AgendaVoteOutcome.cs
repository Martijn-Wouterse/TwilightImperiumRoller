namespace TwilightImperiumRoller.Models;

public class AgendaVoteOutcome
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Label { get; set; } = string.Empty;

  // Keyed by the anonymous per-browser-tab session guid (Components/AgendaVoting.razor.cs).
  // Casting again from the same session replaces its prior entry rather than stacking.
  public Dictionary<Guid, int> Contributions { get; set; } = [];
}
