using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public class GameStateService : IGameStateService
{
  private readonly IGameStateRepository _repository;
  private readonly SemaphoreSlim _initLock = new(1, 1);
  private bool _initialized;

  public GameStateService(IGameStateRepository repository)
  {
    _repository = repository;
  }

  public GameStateModel Current { get; private set; } = new();

  public event Action? Changed;

  public async Task InitializeAsync()
  {
    if (_initialized)
    {
      return;
    }

    await _initLock.WaitAsync();
    try
    {
      if (_initialized)
      {
        return;
      }

      var loaded = await _repository.LoadAsync();
      if (loaded is null)
      {
        Current = CreateSeedState();
        await _repository.SaveAsync(Current);
      }
      else
      {
        Current = loaded;
      }

      _initialized = true;
    }
    finally
    {
      _initLock.Release();
    }
  }

  public async Task SaveAsync()
  {
    await _repository.SaveAsync(Current);
    Changed?.Invoke();
  }

  public IEnumerable<Objective> GetObjectivesForPlayer(Guid playerId) =>
    Current.Objectives.Where(o => o.OwnerPlayerId == playerId || o.ScoredPlayerIds.Contains(playerId));

  public IEnumerable<Objective> GetObjectivesForRound(GameRound round) =>
    Current.Objectives.Where(o => round.ObjectiveIds.Contains(o.Id));

  public int GetPoints(Guid playerId) => GetObjectivesForPlayer(playerId).Sum(o => o.Points);

  public async Task<GameRound> AddRoundAsync()
  {
    var round = new GameRound { RoundNumber = Current.Rounds.Count + 1 };
    Current.Rounds.Add(round);
    await SaveAsync();
    return round;
  }

  public async Task<Objective> AddSecretObjectiveAsync(Guid playerId, string name, string description, int points)
  {
    var objective = new Objective
    {
      Name = name,
      Description = description,
      Points = points,
      IsSecret = true,
      OwnerPlayerId = playerId,
    };
    objective.ScoredPlayerIds.Add(playerId);

    Current.Objectives.Add(objective);
    await SaveAsync();
    return objective;
  }

  public async Task<Objective> AddRoundObjectiveAsync(Guid roundId, string name, string description, int points, bool isSecret)
  {
    var round = Current.Rounds.First(r => r.Id == roundId);
    var objective = new Objective
    {
      Name = name,
      Description = description,
      Points = points,
      IsSecret = isSecret,
    };

    Current.Objectives.Add(objective);
    round.ObjectiveIds.Add(objective.Id);
    await SaveAsync();
    return objective;
  }

  public async Task UnassignObjectiveFromPlayerAsync(Guid objectiveId, Guid playerId)
  {
    var objective = Current.Objectives.FirstOrDefault(o => o.Id == objectiveId);
    if (objective is null)
    {
      return;
    }

    objective.ScoredPlayerIds.Remove(playerId);
    if (objective.OwnerPlayerId == playerId)
    {
      // A secret objective belongs to exactly one player; nothing else can reference it once unassigned.
      Current.Objectives.Remove(objective);
    }

    await SaveAsync();
  }

  public async Task DeleteRoundObjectiveAsync(Guid objectiveId)
  {
    var round = Current.Rounds.FirstOrDefault(r => r.ObjectiveIds.Contains(objectiveId));
    round?.ObjectiveIds.Remove(objectiveId);
    Current.Objectives.RemoveAll(o => o.Id == objectiveId);
    await SaveAsync();
  }

  public async Task ToggleObjectiveScoredAsync(Guid objectiveId, Guid playerId)
  {
    var objective = Current.Objectives.FirstOrDefault(o => o.Id == objectiveId);
    if (objective is null)
    {
      return;
    }

    if (!objective.ScoredPlayerIds.Remove(playerId))
    {
      objective.ScoredPlayerIds.Add(playerId);
    }

    await SaveAsync();
  }

  public async Task<Agenda> AddAgendaAsync(string name, string? elect, string effect)
  {
    var agenda = new Agenda { Name = name, Elect = elect, Effect = effect };
    Current.Agenda.Add(agenda);
    await SaveAsync();
    return agenda;
  }

  public async Task RemoveAgendaAsync(Guid agendaId)
  {
    Current.Agenda.RemoveAll(a => a.Id == agendaId);
    await SaveAsync();
  }

  private static GameStateModel CreateSeedState() => new()
  {
    Players =
    [
      new() { Name = "Martijn", Color = "red", Faction = EFactions.Letnev },
      new() { Name = "Daan", Color = "blue", Faction = EFactions.Sol },
      new() { Name = "Tim", Color = "green", Faction = EFactions.Arborec },
      new() { Name = "Tjalling", Color = "purple", Faction = EFactions.Empyrean },
      new() { Name = "Rixt", Color = "yellow", Faction = EFactions.Hacan },
      new() { Name = "Robert", Color = "black", Faction = EFactions.Winnu },
    ],
    Rounds = [new() { RoundNumber = 1 }],
  };
}
