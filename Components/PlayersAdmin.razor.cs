using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using TwilightImperiumRoller.Models;
using TwilightImperiumRoller.Services;

namespace TwilightImperiumRoller.Components;

public partial class PlayersAdmin : ComponentBase, IDisposable
{
  [Inject]
  private IGameStateService GameStateService { get; set; } = null!;

  private GameStateModel GameStateModel => GameStateService.Current;
  private List<Player> Players => GameStateModel.Players;
  private const int MaxPlayers = 8;


  public void Dispose()
  {

  }

  private void HandleSubmit()
  {
    // model.Name contains the entered text
  }

  private Task OnPlayerNameChanged(Player player, string name) =>
    GameStateService.UpdatePLayerNameAsync(player.Id, name);

  private Task OnPlayerColorChanged(Player player, PlayerColor color) =>
    GameStateService.UpdatePlayerColorAsync(player.Id, color);

  private Task OnPlayerFactionChanged(Player player, Faction faction) =>
    GameStateService.UpdatePlayerFactionAsync(player.Id, faction);

  private Task OnDeletePlayer(Player player) =>
    GameStateService.DeletePlayerAsync(player.Id);

  private async Task OnAddPlayer()
  {
    if (Players.Count >= MaxPlayers)
    {
      return;
    }

    await GameStateService.AddNewPlayerAsync();
  }

  private static string StyleFor(PlayerColor color) => $"background-color:color-mix(in srgb, {color} 60%, white);";

  private static string GetDescription(Faction faction) => typeof(Faction)
      .GetField(faction.ToString())?
      .GetCustomAttribute<DescriptionAttribute>()?
      .Description ?? faction.ToString();

  private static string? GetIcon(Faction faction) =>
    typeof(Faction)
      .GetField(faction.ToString())?
      .GetCustomAttribute<FactionLinkAttribute>()?
      .Link;
}
