using Microsoft.AspNetCore.Components;
using TwilightImperiumRoller.Models;
using TwilightImperiumRoller.Services;

namespace TwilightImperiumRoller.Components;

public partial class DiceRoller
{
  [Inject]
  private IDiceRollingService DiceRollingService { get; set; } = null!;

  public List<ShipStats> ShipStats { get; set; } = new();

  protected override void OnInitialized()
  {
    ShipStats =
    [
      new() { Name = "Flagship", Dice = 0, Hit = 8, Path = "/images/Flagship_Plastic.webp" },
      new() { Name = "Warsun", Dice = 0, Hit = 3, Path = "/images/War_Sun_Plastic.webp" },
      new() { Name = "Dreadnought", Dice = 0, Hit = 5, Path = "/images/Dreadnought_Plastic.webp" },
      new() { Name = "Cruiser", Dice = 0, Hit = 7, Path = "/images/Cruiser_Plastic.webp" },
      new() { Name = "Carrier", Dice = 0, Hit = 9, Path = "/images/Carrier_Plastic.webp" },
      new() { Name = "Destroyer", Dice = 0, Hit = 9, Path = "/images/Destroyer_Plastic.webp" },
      new() { Name = "Fighter", Dice = 0, Hit = 9, Path = "/images/Fighter_Plastic.webp" },
      new() { Name = "Mech", Dice = 0, Hit = 8, Path = "/images/Mech_Plastic.webp" },
      new() { Name = "Infantry", Dice = 0, Hit = 8, Path = "/images/Infantry_Plastic.webp" },
      new() { Name = "Space Cannon", Dice = 0, Hit = 6, Path = "/images/PDS_Plastic.webp" },
    ];
  }

  private int? rollResult;
  private string overlayVisible = "invisible";
  private bool waitingForRoll;
  private int totalDiceInPool;

  private async Task RollButtonOnClick()
  {
    rollResult = 0;
    waitingForRoll = true;

    ShipStats.ForEach(s => s.Results.Clear());

    totalDiceInPool = ShipStats.Sum(s => s.Dice);

    if (totalDiceInPool == 0)
    {
      waitingForRoll = false;
      return;
    }

    List<Task> tasks = [];

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    DisplayResult();
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

  private async Task DisplayResult()
  {
    var highestResult = 0;
    while (waitingForRoll)
    {
      int hits = DiceRollingService.CountHits(ShipStats);
      if (hits > highestResult)
      {
        rollResult = hits;
        highestResult = hits;
        StateHasChanged();
      }

      await Task.Delay(10);
    }

    // Final pass in case the last hit landed between the loop's last check and waitingForRoll flipping false.
    int finalHits = DiceRollingService.CountHits(ShipStats);
    if (finalHits > highestResult)
    {
      rollResult = finalHits;
      StateHasChanged();
    }
  }

  private async Task DiceRoll(Result result)
  {
    TimeSpan rollDuration = DiceRollingService.GetRollDuration(totalDiceInPool);
    DateTime start = DateTime.UtcNow;

    while (DateTime.UtcNow - start < rollDuration)
    {
      result.DisplayValue = DiceRollingService.RollDie();
      StateHasChanged();

      TimeSpan elapsed = DateTime.UtcNow - start;
      int delay = DiceRollingService.GetTickDelayMilliseconds(elapsed, rollDuration);
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
