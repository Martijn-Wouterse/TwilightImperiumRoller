namespace TwilightImperiumRoller.Models;

public class AgendaVote
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string AgendaName { get; set; } = string.Empty;
  public string? Elect { get; set; }
  public string Effect { get; set; } = string.Empty;
  public List<AgendaVoteOutcome> Outcomes { get; set; } = [];
}
