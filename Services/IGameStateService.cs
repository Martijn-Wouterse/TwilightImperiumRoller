using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public interface IGameStateService
{
  GameStateModel Current { get; }

  /// <summary>Raised after any mutation has been persisted, so subscribers can refresh without polling.</summary>
  event Action? Changed;

  Task InitializeAsync();

  /// <summary>Persists <see cref="Current"/> as-is and raises <see cref="Changed"/>. Use after in-place edits (e.g. player color/faction).</summary>
  Task SaveAsync();

  IEnumerable<Objective> GetObjectivesForPlayer(Guid playerId);
  IEnumerable<Objective> GetObjectivesForRound(GameRound round);
  int GetPoints(Guid playerId);

  Task<GameRound> AddRoundAsync();
  Task<Objective> AddSecretObjectiveAsync(Guid playerId, string name, string description, int points);
  Task<Objective> AddRoundObjectiveAsync(Guid roundId, string name, string description, int points, bool isSecret);
  Task UnassignObjectiveFromPlayerAsync(Guid objectiveId, Guid playerId);
  Task DeleteRoundObjectiveAsync(Guid objectiveId);
  Task ToggleObjectiveScoredAsync(Guid objectiveId, Guid playerId);

  Task<Agenda> AddAgendaAsync(string name, string? elect, string effect);
  Task RemoveAgendaAsync(Guid agendaId);
}
