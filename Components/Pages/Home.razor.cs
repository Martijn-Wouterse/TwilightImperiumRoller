using Microsoft.AspNetCore.Components;

namespace TwilightImperiumRoller.Components.Pages;

public partial class Home : ComponentBase
{
  [SupplyParameterFromQuery]
  public string? Password { get; set; } = string.Empty;

  private bool isAdmin;

  protected override void OnInitialized()
  {
    if (Password == "morph123")
    {
      isAdmin = true;
    }
  }
}
