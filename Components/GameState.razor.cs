using Microsoft.AspNetCore.Components;
using TwilightImperiumRoller.Models;
using TwilightImperiumRoller.Services;

namespace TwilightImperiumRoller.Components;

public partial class GameState : ComponentBase, IDisposable
{
  [Inject]
  private IGameStateService GameStateService { get; set; } = null!;

  [Parameter]
  public bool IsAdmin { get; set; }

  private GameStateModel GameStateModel => GameStateService.Current;
  private string overlayVisible = "invisible";
  private Player? editingPlayer;
  private GameRound? editingRound;
  private Objective newObjective = new();
  private IEnumerable<EFactions> Factions { get; } = Enum.GetValues<EFactions>();

  protected override void OnInitialized()
  {
    GameStateService.Changed += OnGameStateChanged;
  }

  private void OnGameStateChanged() => InvokeAsync(StateHasChanged);

  private void OnAddObjective(Player player)
  {
    overlayVisible = string.Empty;
    newObjective = new();
    editingPlayer = player;
    editingRound = null;
  }

  private void OnAddObjective(GameRound round)
  {
    overlayVisible = string.Empty;
    newObjective = new();
    editingRound = round;
    editingPlayer = null;
  }

  private async Task OnConfirmObjective()
  {
    if (editingPlayer != null)
    {
      await GameStateService.AddSecretObjectiveAsync(editingPlayer.Id, newObjective.Name, newObjective.Description, newObjective.Points);
    }
    else if (editingRound != null)
    {
      await GameStateService.AddRoundObjectiveAsync(editingRound.Id, newObjective.Name, newObjective.Description, newObjective.Points, newObjective.IsSecret);
    }

    HideOverlay();
  }

  private Task RemoveObjective(Objective objective, Player? player = null) =>
    player != null
      ? GameStateService.UnassignObjectiveFromPlayerAsync(objective.Id, player.Id)
      : GameStateService.DeleteRoundObjectiveAsync(objective.Id);

  private Task AddRound() => GameStateService.AddRoundAsync();

  private void HideOverlay()
  {
    overlayVisible = "invisible";
    newObjective = new();
    editingPlayer = null;
    editingRound = null;
    StateHasChanged();
  }

  private Task OnGetObjective(Objective objective, Player player) =>
    IsAdmin ? GameStateService.ToggleObjectiveScoredAsync(objective.Id, player.Id) : Task.CompletedTask;

  private Task OnPlayerColorChanged(Player player, string color)
  {
    player.Color = color;
    return GameStateService.SaveAsync();
  }

  private Task OnPlayerFactionChanged(Player player, EFactions faction)
  {
    player.Faction = faction;
    return GameStateService.SaveAsync();
  }

  public void Dispose()
  {
    GameStateService.Changed -= OnGameStateChanged;
  }
}
