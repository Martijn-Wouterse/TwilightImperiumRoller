using Microsoft.AspNetCore.Components;
using TwilightImperiumRoller.Models;
using TwilightImperiumRoller.Services;

namespace TwilightImperiumRoller.Components;

public partial class Agendas : ComponentBase, IDisposable
{
  [Parameter]
  public bool IsAdmin { get; set; }

  [Inject]
  private IGameStateService GameStateService { get; set; } = null!;

  [Inject]
  private IAgendaCatalogService AgendaCatalog { get; set; } = null!;

  private GameStateModel GameStateModel => GameStateService.Current;
  private Agenda newAgenda = new();
  private string overlayVisible = "invisible";
  private IEnumerable<string> AgendaNameSuggestions => AgendaCatalog.All.Select(a => a.Name);

  protected override void OnInitialized()
  {
    GameStateService.Changed += OnGameStateChanged;
  }

  private void OnGameStateChanged() => InvokeAsync(StateHasChanged);

  private Task RemoveAgenda(Agenda agenda) => GameStateService.RemoveAgendaAsync(agenda.Id);

  private void OnAddAgenda()
  {
    overlayVisible = string.Empty;
    newAgenda = new();
  }

  private void OnAgendaNameChanged(object value)
  {
    AgendaCardDefinition? match = AgendaCatalog.FindByName(value as string ?? string.Empty);
    newAgenda.Elect = match?.Elect;
    newAgenda.Effect = match?.Effect ?? string.Empty;
  }

  private async Task OnConfirmAgenda()
  {
    await GameStateService.AddAgendaAsync(newAgenda.Name, newAgenda.Elect, newAgenda.Effect);
    HideOverlay();
  }

  private void HideOverlay()
  {
    overlayVisible = "invisible";
    newAgenda = new();
    StateHasChanged();
  }

  public void Dispose()
  {
    GameStateService.Changed -= OnGameStateChanged;
  }
}
