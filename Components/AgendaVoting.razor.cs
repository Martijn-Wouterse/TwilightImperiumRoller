using Microsoft.AspNetCore.Components;
using TwilightImperiumRoller.Models;
using TwilightImperiumRoller.Services;

namespace TwilightImperiumRoller.Components;

public partial class AgendaVoting : ComponentBase, IDisposable
{
  [Parameter]
  public bool IsAdmin { get; set; }

  [Inject]
  private IGameStateService GameStateService { get; set; } = null!;

  [Inject]
  private IAgendaCatalogService AgendaCatalog { get; set; } = null!;

  // Anonymous per-tab identity: generated once per component instance (i.e. once per browser
  // tab/circuit), never persisted, never shown. Sent alongside every CastVoteAsync call so a
  // tab's own re-vote replaces its prior contribution instead of stacking.
  private readonly Guid sessionId = Guid.NewGuid();

  private GameStateModel GameStateModel => GameStateService.Current;
  private AgendaVote? ActiveVote => GameStateModel.ActiveVote;

  private string setupOverlayVisible = "invisible";
  private string agendaNameInput = string.Empty;
  private string? electInput;
  private string effectInput = string.Empty;
  private List<OutcomeRow> outcomeRows = CreateDefaultOutcomeRows();
  private IEnumerable<string> AgendaNameSuggestions => AgendaCatalog.All.Select(a => a.Name);

  // Locally-typed (not-yet-submitted) numeric amount per outcome, keyed by outcome id.
  private readonly Dictionary<Guid, int> pendingVoteAmounts = [];
  private Guid? lastSeenVoteId;
  private string newOutcomeLabelInput = string.Empty;

  // Each setup row needs a stable identity independent of its position: rows are added/removed
  // out of order, and without a stable @key Blazor's positional diffing can leave a removal's
  // click handler bound to a stale index from a prior render, throwing on RemoveAt. Using a
  // list-index int as the closure key (the original bug) doesn't fix this since the index itself
  // shifts on removal - only a per-row identity that never changes does.
  private sealed class OutcomeRow
  {
    public Guid Id { get; } = Guid.NewGuid();
    public string Label { get; set; } = string.Empty;
  }

  private static List<OutcomeRow> CreateDefaultOutcomeRows() =>
  [
    new() { Label = "For" },
    new() { Label = "Against" },
  ];

  protected override void OnInitialized()
  {
    lastSeenVoteId = ActiveVote?.Id;
    GameStateService.Changed += OnGameStateChanged;
  }

  private void OnGameStateChanged()
  {
    // A new voting round (or the round ending) invalidates any locally-typed-but-unsubmitted
    // amounts; a live total update from another tab's vote should not clobber what this tab is
    // mid-typing, so only clear pending amounts on an actual round transition.
    Guid? currentVoteId = ActiveVote?.Id;
    if (currentVoteId != lastSeenVoteId)
    {
      pendingVoteAmounts.Clear();
      lastSeenVoteId = currentVoteId;
    }

    InvokeAsync(StateHasChanged);
  }

  private void OnStartVoteClick()
  {
    agendaNameInput = string.Empty;
    electInput = null;
    effectInput = string.Empty;
    outcomeRows = CreateDefaultOutcomeRows();
    setupOverlayVisible = string.Empty;
  }

  private void OnAgendaNameChanged(object value)
  {
    AgendaCardDefinition? match = AgendaCatalog.FindByName(value as string ?? string.Empty);
    electInput = match?.Elect;
    effectInput = match?.Effect ?? string.Empty;
  }

  private void AddOutcomeRow() => outcomeRows.Add(new OutcomeRow());

  private void RemoveOutcomeRow(Guid rowId)
  {
    outcomeRows.RemoveAll(r => r.Id == rowId);

    // RadzenIcon has no Click parameter of its own; the onclick handler below is splatted onto
    // its internal render tree via RadzenComponent's Attributes dictionary, so Blazor's automatic
    // post-event re-render targets RadzenIcon rather than this component. Force it explicitly,
    // same as HideSetupOverlay does for the same reason.
    StateHasChanged();
  }

  private async Task OnConfirmStartVote()
  {
    List<string> labels = outcomeRows.Select(r => r.Label).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
    if (string.IsNullOrWhiteSpace(agendaNameInput) || labels.Count == 0)
    {
      return;
    }

    await GameStateService.StartVoteAsync(agendaNameInput, electInput, effectInput, labels);
    HideSetupOverlay();
  }

  private void HideSetupOverlay()
  {
    setupOverlayVisible = "invisible";
    StateHasChanged();
  }

  private int GetPendingAmount(Guid outcomeId) =>
    pendingVoteAmounts.TryGetValue(outcomeId, out int value) ? value : 0;

  private void SetPendingAmount(Guid outcomeId, int value) => pendingVoteAmounts[outcomeId] = value;

  private Task OnCastVote(Guid outcomeId) =>
    GameStateService.CastVoteAsync(outcomeId, sessionId, GetPendingAmount(outcomeId));

  private async Task OnAddVoteOutcome()
  {
    if (string.IsNullOrWhiteSpace(newOutcomeLabelInput))
    {
      return;
    }

    await GameStateService.AddVoteOutcomeAsync(newOutcomeLabelInput);
    newOutcomeLabelInput = string.Empty;
  }

  private Task OnStopVote() => GameStateService.StopVoteAsync();

  public void Dispose()
  {
    GameStateService.Changed -= OnGameStateChanged;
  }
}
