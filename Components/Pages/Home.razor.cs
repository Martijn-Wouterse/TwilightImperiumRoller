using Microsoft.AspNetCore.Components;
using System;
using TwilightImperiumRoller.Models;

namespace TwilightImperiumRoller.Components.Pages
{
  public partial class Home: ComponentBase, IDisposable
  {
    [Parameter]
    public string? Password { get; set; } = string.Empty;
    [Inject]
    private TimeProvider timeProvider { get; set; } = null!;
    private GameState gameState { get; set; } = null!;
    private Agendas agendas { get; set; } = null!;
    private bool isAdmin { get; set; } = false;
    private bool disposedValue;
    private ITimer? timer;

    protected override void OnInitialized()
    {
      if (Password == "morph123")
      {
        isAdmin = true;
      }
    }

    private async void TimerHandler(object? state)
    {
      await gameState.GetGameState();
      await agendas.GetGameState();
    }

    protected override void OnAfterRender(bool firstRender)
    {
      if (firstRender && timer == null)
      {
        timer = timeProvider.CreateTimer(TimerHandler, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          timer?.Dispose();
        }

        disposedValue = true;
      }
    }

    public void Dispose()
    {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}