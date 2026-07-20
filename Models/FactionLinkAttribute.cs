namespace TwilightImperiumRoller.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class FactionLinkAttribute : Attribute
{
  public FactionLinkAttribute(string text)
  {
    Link = text;
  }

  public string Link { get; }
}
