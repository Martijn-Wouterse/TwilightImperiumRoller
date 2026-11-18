using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public interface IGameStateService
{
  GameStateModel Current { get; }

  /// <summary>Raised after any mutation has been persisted, so subscribers can refresh without polling.</summary>
  event Action? Changed;

  Task InitializeAsync();

  Task AddNewPlayerAsync();
  Task UpdatePLayerNameAsync(Guid playerId, string name);
  Task UpdatePlayerColorAsync(Guid playerId, PlayerColor color);
  Task UpdatePlayerFactionAsync(Guid playerId, Faction faction);
  Task DeletePlayerAsync(Guid playerId);

  IEnumerable<Objective> GetObjectivesForPlayer(Guid playerId);
  IEnumerable<Objective> GetObjectivesForRound(GameRound round);
  int GetPoints(Guid playerId);

  Task<GameRound> AddRoundAsync();
  Task<Objective> AddSecretObjectiveAsync(Guid playerId, string name, string condition, int points, string source = "");
  Task<Objective> AddRoundObjectiveAsync(Guid roundId, string name, string condition, int points, bool isSecret, string source = "");
  Task UnassignObjectiveFromPlayerAsync(Guid objectiveId, Guid playerId);
  Task DeleteRoundObjectiveAsync(Guid objectiveId);
  Task ToggleObjectiveScoredAsync(Guid objectiveId, Guid playerId);

  Task<Agenda> AddAgendaAsync(string name, string? elect, string effect);
  Task RemoveAgendaAsync(Guid agendaId);

  Task<AgendaVote> StartVoteAsync(string agendaName, string? elect, string effect, IReadOnlyList<string> outcomeLabels);
  Task CastVoteAsync(Guid outcomeId, Guid sessionId, int amount);
  Task AddVoteOutcomeAsync(string label);
  Task StopVoteAsync();
}
