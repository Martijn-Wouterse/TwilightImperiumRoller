namespace TwilightImperiumRoller.Models
{
  public class GameStateModel
  {
    public List<Player> Players { get; set; } = [];
    public List<GameRound> Rounds { get; set; } = [];
    public List<Agenda> Agenda { get; set; } = [];
  }

  public class GameRound
  {
    public int RoundNumber { get; set; }
    public List<Objective> Objectives { get; set; } = [];
  }

  public class Objective
  {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Player> ScoredPlayers { get; set; } = [];
    public int Points { get; set; } = 1;
    public bool IsSecret { get; set; }
  }

  public class Player
  {
    public string Name { get; set; } = string.Empty;
    public List<Objective> Objectives { get; set; } = [];
    public int Points { get { return Objectives.Sum(x => x.Points); } }
    public EFactions Faction { get; set; }
    public string Color { get; set; } = string.Empty;
  }

  public class Agenda
  {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
  }
}
