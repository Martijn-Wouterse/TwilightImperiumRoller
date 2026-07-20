using System.Text.Json;
using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public class JsonFileGameStateRepository : IGameStateRepository
{
  private readonly string _filePath;
  private readonly ILogger<JsonFileGameStateRepository> _logger;
  private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

  public JsonFileGameStateRepository(IWebHostEnvironment environment, ILogger<JsonFileGameStateRepository> logger)
  {
    _filePath = Path.Combine(environment.ContentRootPath, "Data", "game-state.json");
    _logger = logger;
  }

  public async Task<GameStateModel?> LoadAsync()
  {
    if (!File.Exists(_filePath))
    {
      return null;
    }

    try
    {
      await using var stream = File.OpenRead(_filePath);
      return await JsonSerializer.DeserializeAsync<GameStateModel>(stream, _options);
    }
    catch (JsonException ex)
    {
      string backupPath = _filePath + ".bak";
      _logger.LogError(ex, "Game state file at {FilePath} could not be parsed. Backing it up to {BackupPath} and starting fresh.", _filePath, backupPath);
      File.Copy(_filePath, backupPath, overwrite: true);
      return null;
    }
  }

  public async Task SaveAsync(GameStateModel model)
  {
    string json = JsonSerializer.Serialize(model, _options);
    await File.WriteAllTextAsync(_filePath, json);
  }
}
