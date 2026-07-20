using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Components
{
  public partial class GameState
  {
    private GameStateModel GameStateModel { get; set; } = new();
    private string filePath = "./game-state.json";
    private JsonSerializerOptions options = new JsonSerializerOptions
    {
      ReferenceHandler = ReferenceHandler.Preserve,
      WriteIndented = true 
    };
    [Parameter]
    public bool IsAdmin { get; set; }
    private string overlayVisible = "invisible";
    private Player? editingPlayer = null;
    private GameRound? editingRound = null;
    private Objective newObjective { get; set; } = new();
    private IEnumerable<EFactions> Factions { get; set; } = Enum.GetValues(typeof(EFactions)).Cast<EFactions>();
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

    private async Task OnAddObjective(Player? player = null)
    {
      overlayVisible = string.Empty;
      newObjective = new();
      editingPlayer = player;
      await StoreGameState();
      StateHasChanged();
    }

    private async Task OnAddObjective(GameRound? round, Player? player = null)
    {
      overlayVisible = string.Empty;
      newObjective = new();
      editingPlayer = player;
      editingRound = round;
      await StoreGameState();
      StateHasChanged();
    }

    private async Task OnConfirmObjective()
    {
      if (editingPlayer != null)
      {
        newObjective.IsSecret = true;
        GameStateModel.Players.First(p => p.Name == editingPlayer.Name).Objectives.Add(newObjective);
      }
      else if (editingRound != null)
      {
        GameStateModel.Rounds.First(r => r.RoundNumber == editingRound.RoundNumber).Objectives.Add(newObjective);
      }
      else
      {

        GameStateModel.Rounds.Last().Objectives.Add(newObjective);
      }
      await StoreGameState();
      HideOverlay();
      StateHasChanged();
    }

    private async Task RemoveObjective(Objective objective, Player? player = null)
    {
      if (player != null)
      {
        player.Objectives.Remove(objective);
        foreach (var round in GameStateModel.Rounds)
        {
          if (round.Objectives.Any(o => o.Name == objective.Name))
          {
            var correctObjective = round.Objectives.FirstOrDefault(o => o.Name == objective.Name);
            correctObjective?.ScoredPlayers.Remove(player);
          }
        }
      }
      else
      {
        foreach (var round in GameStateModel.Rounds)
        {
          if (round.Objectives.Contains(objective))
          {
            round.Objectives.Remove(objective);
            foreach (var gamePlayers in GameStateModel.Players)
            {
              if (gamePlayers.Objectives.Contains(objective))
              {
                gamePlayers.Objectives.Remove(objective);
              }
            }
          }
        }
      }
      await StoreGameState();
      StateHasChanged();
    }

    private async Task AddRound()
    {
      GameStateModel.Rounds.Add(new GameRound() { RoundNumber = GameStateModel.Rounds.Count + 1 });
      await StoreGameState();
      StateHasChanged();
    }

    private void HideOverlay()
    {
      overlayVisible = "invisible";
      newObjective = new();
      editingPlayer = null;
      editingRound = null;
      StateHasChanged();
    }

    private async Task OnGetObjective(Objective objective, Player player)
    {
      if (!IsAdmin)
      {
        return;
      }
      if (objective.ScoredPlayers.Contains(player))
      {
        objective.ScoredPlayers.Remove(player);
        player.Objectives.Remove(objective);
      }
      else
      {
        objective.ScoredPlayers.Add(player);
        player.Objectives.Add(objective);
      }
      await StoreGameState();
      StateHasChanged();
    }

    private async Task StoreGameState()
    {
      string json = JsonSerializer.Serialize(GameStateModel, options);
      await File.WriteAllTextAsync(filePath, json);
    }
  }
}