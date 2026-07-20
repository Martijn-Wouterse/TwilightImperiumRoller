using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Components
{
  public partial class DiceRoller
  {
    public List<ShipStats> ShipStats { get; set; } = new();

    protected override void OnInitialized()
    {
      ShipStats =
  [
      new() { Name = "Flagship", Dice = 0, Hit = 8, Path = "/images/Flagship_Plastic.webp"},
            new() { Name = "Warsun", Dice = 0, Hit = 3, Path = "/images/War_Sun_Plastic.webp"},
            new() { Name = "Dreadnought", Dice = 0, Hit = 5, Path = "/images/Dreadnought_Plastic.webp"},
            new() { Name = "Cruiser", Dice = 0, Hit = 7, Path = "/images/Cruiser_Plastic.webp"},
            new() { Name = "Carrier", Dice = 0, Hit = 9, Path = "/images/Carrier_Plastic.webp"},
            new() { Name = "Destroyer", Dice = 0, Hit = 9, Path = "/images/Destroyer_Plastic.webp"},
            new() { Name = "Fighter", Dice = 0, Hit = 9, Path = "/images/Fighter_Plastic.webp"},
            new() { Name = "Mech", Dice = 0, Hit = 8, Path = "/images/Mech_Plastic.webp"},
            new() { Name = "Infantry", Dice = 0, Hit = 8, Path = "/images/Infantry_Plastic.webp"},
            new() { Name = "Space Cannon", Dice = 0, Hit = 6, Path = "/images/PDS_Plastic.webp"},
        ];
    }

    private int? rollResult = null;
    private string overlayVisible = "invisible";
    private bool waitingForRoll = false;
    private int totalRolls = 0;

    private async Task RollButtonOnClick()
    {
      List<Task> tasks = [];
      rollResult = 0;

      waitingForRoll = true;

      ShipStats.ForEach(s => s.Results.Clear());

      totalRolls = ShipStats.Sum(s => s.Dice);

      if (!ShipStats.Any(s => s.Dice >= 1))
      {
        return;
      }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
      DisplayResult(waitingForRoll);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

      foreach (var ship in ShipStats.Where(s => s.Dice >= 1))
      {
        for (int i = 0; i < ship.Dice; i++)
        {
          Result newRoll = new();

          ship.Results.Add(newRoll);
          tasks.Add(DiceRoll(newRoll));
          StateHasChanged();
        }
      }

      overlayVisible = string.Empty;
      StateHasChanged();

      await Task.WhenAll(tasks);
      waitingForRoll = false;
    }

    private async Task DisplayResult(bool resultIsIn)
    {
      var highestResult = 0;
      while (resultIsIn)
      {
        int displayResults = 0;
        foreach (ShipStats ship in ShipStats)
        {
          foreach (Result result in ship.Results)
          {
            if (result.FinalValue != null && result.FinalValue >= ship.Hit)
            {
              displayResults++;
            }
          }
        }
        await Task.Delay(10);
        if (displayResults > highestResult)
        {
          rollResult = displayResults;
          highestResult = displayResults;
          StateHasChanged();
        }
      }
    }

    private async Task DiceRoll(Result result)
    {
      Random random = new();
      int max = totalRolls <= 4 ? totalRolls : 4;
      int rollDuration = random.Next(1000, max * 1000);
      DateTime start = DateTime.UtcNow;
      double elapsed = 0.0;

      while ((DateTime.UtcNow - start).TotalMilliseconds < rollDuration)
      {
        result.DisplayValue = random.Next(1, 11);

        StateHasChanged();

        elapsed = (DateTime.UtcNow - start).TotalMilliseconds;
        double t = elapsed / rollDuration;

        double eased = t * t;
        int delay = 40 + (int)(eased * 200);
        await Task.Delay(delay);
      }
      result.FinalValue = result.DisplayValue;
    }

    private void HideOverlay()
    {
      rollResult = null;
      ShipStats.ForEach(s => s.Results.Clear());
      overlayVisible = "invisible";
      StateHasChanged();
    }
  }
}