using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Services;

public interface IObjectiveCatalogService
{
  IReadOnlyList<ObjectiveCardDefinition> All { get; }

  Task InitializeAsync();

  /// <summary>Finds the catalog entry whose name matches exactly (case-insensitive), if any.</summary>
  ObjectiveCardDefinition? FindByName(string name);
}
