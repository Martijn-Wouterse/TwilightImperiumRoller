namespace TwilightImperiumRoller.Models;

public class GameStateModel
{
  public List<Player> Players { get; set; } = [];
  public List<GameRound> Rounds { get; set; } = [];
  public List<Objective> Objectives { get; set; } = [];
  public List<Agenda> Agenda { get; set; } = [];
}

public class GameRound
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public int RoundNumber { get; set; }
  public List<Guid> ObjectiveIds { get; set; } = [];
}

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

public class Player
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;
  public EFactions Faction { get; set; }
  public string Color { get; set; } = string.Empty;
}

public class Agenda
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;
  public string? Elect { get; set; }
  public string Effect { get; set; } = string.Empty;
}
