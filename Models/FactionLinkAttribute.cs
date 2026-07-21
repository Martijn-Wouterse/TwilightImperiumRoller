namespace TwilightImperiumRoller.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class FactionLinkAttribute(string text) : Attribute
{
  public string Link { get; } = text;
}
