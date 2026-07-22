using System.Text.Json;
using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public class AgendaCatalogService(IWebHostEnvironment environment, ILogger<AgendaCatalogService> logger) : IAgendaCatalogService
{
  private readonly string _filePath = Path.Combine(environment.ContentRootPath, "Data", "agendas.json");
  private readonly ILogger<AgendaCatalogService> _logger = logger;
  private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
  private readonly SemaphoreSlim _initLock = new(1, 1);
  private bool _initialized;

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
          await using FileStream stream = File.OpenRead(_filePath);
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
