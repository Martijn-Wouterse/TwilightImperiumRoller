using System.Text.Json;
using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public class ObjectiveCatalogService : IObjectiveCatalogService
{
  private readonly string _filePath;
  private readonly ILogger<ObjectiveCatalogService> _logger;
  private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
  private readonly SemaphoreSlim _initLock = new(1, 1);
  private bool _initialized;

  public ObjectiveCatalogService(IWebHostEnvironment environment, ILogger<ObjectiveCatalogService> logger)
  {
    _filePath = Path.Combine(environment.ContentRootPath, "objectives.json");
    _logger = logger;
  }

  public IReadOnlyList<ObjectiveCardDefinition> All { get; private set; } = [];

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

      if (File.Exists(_filePath))
      {
        try
        {
          await using var stream = File.OpenRead(_filePath);
          All = await JsonSerializer.DeserializeAsync<List<ObjectiveCardDefinition>>(stream, _options) ?? [];
        }
        catch (JsonException ex)
        {
          _logger.LogError(ex, "Objective catalog file at {FilePath} could not be parsed. Autocomplete will be empty.", _filePath);
        }
      }
      else
      {
        _logger.LogWarning("Objective catalog file not found at {FilePath}. Autocomplete will be empty.", _filePath);
      }

      _initialized = true;
    }
    finally
    {
      _initLock.Release();
    }
  }

  public ObjectiveCardDefinition? FindByName(string name) =>
    All.FirstOrDefault(o => string.Equals(o.Name, name, StringComparison.OrdinalIgnoreCase));
}
