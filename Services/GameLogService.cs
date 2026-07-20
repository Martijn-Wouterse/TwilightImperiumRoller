using System.Text;

namespace TwilightImperiumRoller.Services;

public class GameLogService : IGameLogService
{
  private readonly string _filePath;
  private readonly SemaphoreSlim _writeLock = new(1, 1);

  public GameLogService(IWebHostEnvironment environment)
  {
    _filePath = Path.Combine(environment.ContentRootPath, "Data", "game-log.txt");
  }

  public async Task AppendAsync(string message)
  {
    string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";

    await _writeLock.WaitAsync();
    try
    {
      await File.AppendAllTextAsync(_filePath, line, Encoding.UTF8);
    }
    finally
    {
      _writeLock.Release();
    }
  }
}
