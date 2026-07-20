namespace TwilightImperiumRoller.Models;

public class GameStateModel
{
  public List<Player> Players { get; set; } = [];
  public List<GameRound> Rounds { get; set; } = [];
  public List<Objective> Objectives { get; set; } = [];
  public List<Agenda> Agenda { get; set; } = [];
}
