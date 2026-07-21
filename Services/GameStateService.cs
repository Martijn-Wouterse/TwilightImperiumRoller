using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public class GameStateService(IGameStateRepository repository, IGameLogService gameLog) : IGameStateService
{
  private readonly IGameStateRepository _repository = repository;
  private readonly IGameLogService _gameLog = gameLog;
  private readonly SemaphoreSlim _initLock = new(1, 1);
  private bool _initialized;

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

      GameStateModel? loaded = await _repository.LoadAsync();
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

  private async Task SaveAsync()
  {
    await _repository.SaveAsync(Current);
    Changed?.Invoke();
  }

  public async Task UpdatePlayerColorAsync(Guid playerId, string color)
  {
    Player player = Current.Players.First(p => p.Id == playerId);
    player.Color = color;
    await _gameLog.AppendAsync($"Changed {player.Name}'s color to {color}.");
    await SaveAsync();
  }

  public async Task UpdatePlayerFactionAsync(Guid playerId, Faction faction)
  {
    Player player = Current.Players.First(p => p.Id == playerId);
    player.Faction = faction;
    await _gameLog.AppendAsync($"Changed {player.Name}'s faction to {faction.GetFactionName()}.");
    await SaveAsync();
  }

  public IEnumerable<Objective> GetObjectivesForPlayer(Guid playerId) =>
    Current.Objectives.Where(o => o.OwnerPlayerId == playerId || o.ScoredPlayerIds.Contains(playerId));

  public IEnumerable<Objective> GetObjectivesForRound(GameRound round) =>
    Current.Objectives.Where(o => round.ObjectiveIds.Contains(o.Id));

  public int GetPoints(Guid playerId) => GetObjectivesForPlayer(playerId).Sum(o => o.Points);

  public async Task<GameRound> AddRoundAsync()
  {
    GameRound round = new() { RoundNumber = Current.Rounds.Count + 1 };
    Current.Rounds.Add(round);
    await _gameLog.AppendAsync($"Added Round {round.RoundNumber}.");
    await SaveAsync();
    return round;
  }

  public async Task<Objective> AddSecretObjectiveAsync(Guid playerId, string name, string condition, int points, string source = "")
  {
    Player player = Current.Players.First(p => p.Id == playerId);
    Objective objective = new()
    {
      Name = name,
      Condition = condition,
      Points = points,
      IsSecret = true,
      Source = source,
      OwnerPlayerId = playerId,
    };
    objective.ScoredPlayerIds.Add(playerId);

    Current.Objectives.Add(objective);
    await _gameLog.AppendAsync($"Added secret objective \"{name}\" ({points} pt) for {player.Name}.");
    await SaveAsync();
    return objective;
  }

  public async Task<Objective> AddRoundObjectiveAsync(Guid roundId, string name, string condition, int points, bool isSecret, string source = "")
  {
    GameRound round = Current.Rounds.First(r => r.Id == roundId);
    Objective objective = new()
    {
      Name = name,
      Condition = condition,
      Points = points,
      IsSecret = isSecret,
      Source = source,
    };

    Current.Objectives.Add(objective);
    round.ObjectiveIds.Add(objective.Id);
    string type = isSecret ? "secret" : "public";
    await _gameLog.AppendAsync($"Added {type} objective \"{name}\" ({points} pt) to Round {round.RoundNumber}.");
    await SaveAsync();
    return objective;
  }

  public async Task UnassignObjectiveFromPlayerAsync(Guid objectiveId, Guid playerId)
  {
    Objective? objective = Current.Objectives.FirstOrDefault(o => o.Id == objectiveId);
    if (objective is null)
    {
      return;
    }

    Player player = Current.Players.First(p => p.Id == playerId);
    objective.ScoredPlayerIds.Remove(playerId);
    if (objective.OwnerPlayerId == playerId)
    {
      // A secret objective belongs to exactly one player; nothing else can reference it once unassigned.
      Current.Objectives.Remove(objective);
      await _gameLog.AppendAsync($"Removed secret objective \"{objective.Name}\" from {player.Name}.");
    }
    else
    {
      await _gameLog.AppendAsync($"Unmarked objective \"{objective.Name}\" as scored by {player.Name}.");
    }

    await SaveAsync();
  }

  public async Task DeleteRoundObjectiveAsync(Guid objectiveId)
  {
    Objective? objective = Current.Objectives.FirstOrDefault(o => o.Id == objectiveId);
    GameRound? round = Current.Rounds.FirstOrDefault(r => r.ObjectiveIds.Contains(objectiveId));
    round?.ObjectiveIds.Remove(objectiveId);
    Current.Objectives.RemoveAll(o => o.Id == objectiveId);
    if (objective is not null && round is not null)
    {
      await _gameLog.AppendAsync($"Removed objective \"{objective.Name}\" from Round {round.RoundNumber}.");
    }

    await SaveAsync();
  }

  public async Task ToggleObjectiveScoredAsync(Guid objectiveId, Guid playerId)
  {
    Objective? objective = Current.Objectives.FirstOrDefault(o => o.Id == objectiveId);
    if (objective is null)
    {
      return;
    }

    Player player = Current.Players.First(p => p.Id == playerId);
    if (!objective.ScoredPlayerIds.Remove(playerId))
    {
      objective.ScoredPlayerIds.Add(playerId);
      await _gameLog.AppendAsync($"{player.Name} scored objective \"{objective.Name}\" ({objective.Points} pt).");
    }
    else
    {
      await _gameLog.AppendAsync($"{player.Name} unscored objective \"{objective.Name}\".");
    }

    await SaveAsync();
  }

  public async Task<Agenda> AddAgendaAsync(string name, string? elect, string effect)
  {
    Agenda agenda = new() { Name = name, Elect = elect, Effect = effect };
    Current.Agenda.Add(agenda);
    await _gameLog.AppendAsync($"Added agenda \"{name}\".");
    await SaveAsync();
    return agenda;
  }

  public async Task RemoveAgendaAsync(Guid agendaId)
  {
    Agenda? agenda = Current.Agenda.FirstOrDefault(a => a.Id == agendaId);
    Current.Agenda.RemoveAll(a => a.Id == agendaId);
    if (agenda is not null)
    {
      await _gameLog.AppendAsync($"Removed agenda \"{agenda.Name}\".");
    }

    await SaveAsync();
  }

  private static GameStateModel CreateSeedState() => new()
  {
    Players =
    [
      new() { Name = "Martijn", Color = "red", Faction = Faction.Letnev },
      new() { Name = "Daan", Color = "blue", Faction = Faction.Sol },
      new() { Name = "Tim", Color = "green", Faction = Faction.Arborec },
      new() { Name = "Tjalling", Color = "purple", Faction = Faction.Empyrean },
      new() { Name = "Rixt", Color = "yellow", Faction = Faction.Hacan },
      new() { Name = "Robert", Color = "black", Faction = Faction.Winnu },
    ],
    Rounds = [new() { RoundNumber = 1 }],
  };
}
