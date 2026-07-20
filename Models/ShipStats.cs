namespace TwilightImperiumRoller.Models;

public class ShipStats
{
  public string Name { get; set; } = string.Empty;
  public int Dice { get; set; }
  public int Hit { get; set; }
  public List<DieRoll> Results { get; set; } = [];
  public string Path { get; set; } = string.Empty;
}

public class DieRoll
{
  public int DisplayValue { get; set; }
  public int? FinalValue { get; set; }
}
