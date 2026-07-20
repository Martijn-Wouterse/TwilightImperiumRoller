using System.ComponentModel;
using System.Reflection;

namespace TwilightImperiumRoller.Models;

public static class FactionExtensions
{
  public static string GetFactionLink(this Enum enumValue, Func<string, string>? translationFunction = null)
  {
    var enumValueAsString = enumValue.ToString();
    var val = enumValue.GetType().GetMember(enumValueAsString).FirstOrDefault();
    var enumVal = val?.GetCustomAttribute<FactionLinkAttribute>()?.Link ?? enumValueAsString;

    if (translationFunction != null)
    {
      return translationFunction(enumVal);
    }

    return enumVal;
  }

  public static string GetFactionName(this Enum enumValue, Func<string, string>? translationFunction = null)
  {
    var enumValueAsString = enumValue.ToString();
    var val = enumValue.GetType().GetMember(enumValueAsString).FirstOrDefault();
    var enumVal = val?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumValueAsString;

    if (translationFunction != null)
    {
      return translationFunction(enumVal);
    }

    return enumVal;
  }
}
