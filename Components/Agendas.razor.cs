using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;
using System.Text.Json;
using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Components
{
  public partial class Agendas
  {
    [Parameter]
    public bool IsAdmin { get; set; }
    private GameStateModel GameStateModel { get; set; } = new();
    private string filePath = "./game-state.json";
    private JsonSerializerOptions options = new JsonSerializerOptions
    {
      ReferenceHandler = ReferenceHandler.Preserve,
      WriteIndented = true
    };
    private Agenda newAgenda { get; set; } = new();
    private string overlayVisible = "invisible";

    public async Task RemoveAgenda(Agenda agenda)
    {
      GameStateModel.Agenda.Remove(agenda);
      await StoreGameState();
      StateHasChanged();
    }
    private async Task OnAddAgenda()
    {
      overlayVisible = string.Empty;
      newAgenda = new();
      await StoreGameState();
      StateHasChanged();
    }

    private async Task OnConfirmAgenda()
    {
      GameStateModel.Agenda.Add(newAgenda);
      HideOverlay();
      await StoreGameState();
    }

    private void HideOverlay()
    {
      overlayVisible = "invisible";
      newAgenda = new();
      StateHasChanged();
    }


    private async Task StoreGameState()
    {
      string json = JsonSerializer.Serialize(GameStateModel, options);
      await File.WriteAllTextAsync(filePath, json);
    }

    protected override async Task OnParametersSetAsync()
    {
      await GetGameState();
      await base.OnParametersSetAsync();
    }

    public async Task GetGameState()
    {
      await InvokeAsync(() =>
      {
        try
        {
          if (File.Exists(filePath))
          {
            string jsonString = File.ReadAllText(filePath);
            GameStateModel = new GameStateModel();
            GameStateModel = JsonSerializer.Deserialize<GameStateModel>(jsonString, options);
          }
          else
          {
            GameStateModel = new GameStateModel
            {
              Players = [
                    new() { Name = "Martijn", Color = "red", Faction= EFactions.Letnev},
              new() { Name = "Daan", Color = "blue", Faction = EFactions.Sol},
              new() { Name = "Tim", Color = "green", Faction = EFactions.Arborec},
              new() { Name = "Tjalling", Color = "purple", Faction = EFactions.Empyrean},
              new() { Name = "Rixt", Color = "yellow", Faction = EFactions.Hacan},
              new() { Name = "Robert", Color = "black", Faction = EFactions.Winnu} ],
              Rounds = [
                    new () { RoundNumber = 1, Objectives = []}
                    ]
            };
          }

          StateHasChanged();
        }
        catch (Exception ex)
        {

        }
      });
    }
  }
}