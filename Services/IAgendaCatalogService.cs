using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public interface IAgendaCatalogService
{
  IReadOnlyList<AgendaCardDefinition> All { get; }

  Task InitializeAsync();

  /// <summary>Finds the catalog entry whose name matches exactly (case-insensitive), if any.</summary>
  AgendaCardDefinition? FindByName(string name);
}
