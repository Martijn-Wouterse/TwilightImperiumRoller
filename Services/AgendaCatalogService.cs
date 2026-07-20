using System.Text.Json;
using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public class AgendaCatalogService : IAgendaCatalogService
{
  private readonly string _filePath;
  private readonly ILogger<AgendaCatalogService> _logger;
  private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
  private readonly SemaphoreSlim _initLock = new(1, 1);
  private bool _initialized;

  public AgendaCatalogService(IWebHostEnvironment environment, ILogger<AgendaCatalogService> logger)
  {
    _filePath = Path.Combine(environment.ContentRootPath, "Data", "agendas.json");
    _logger = logger;
  }

  public IReadOnlyList<AgendaCardDefinition> All { get; private set; } = [];

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
          All = await JsonSerializer.DeserializeAsync<List<AgendaCardDefinition>>(stream, _options) ?? [];
        }
        catch (JsonException ex)
        {
          _logger.LogError(ex, "Agenda catalog file at {FilePath} could not be parsed. Autocomplete will be empty.", _filePath);
        }
      }
      else
      {
        _logger.LogWarning("Agenda catalog file not found at {FilePath}. Autocomplete will be empty.", _filePath);
      }

      _initialized = true;
    }
    finally
    {
      _initLock.Release();
    }
  }

  public AgendaCardDefinition? FindByName(string name) =>
    All.FirstOrDefault(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase));
}
