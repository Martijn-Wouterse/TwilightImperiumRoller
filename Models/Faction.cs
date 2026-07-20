using System.ComponentModel;

namespace TwilightImperiumRoller.Models;

public enum Faction
{
  [Description("Arborec")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/8/8f/ArborecSymbolSquare.png/revision/latest/scale-to-width-down/55?cb=20250928223912")]
  Arborec,
  [Description("Barony of Letnev")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/2/20/Barony.png/revision/latest/scale-to-width-down/55?cb=20201104005247")]
  Letnev,
  [Description("Clans van Saar")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/b/b0/Saar.png/revision/latest/scale-to-width-down/55?cb=20201104005333")]
  Saar,
  [Description("Embers of Muaat")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/3/37/MuaatSymbolSquare.png/revision/latest/scale-to-width-down/55?cb=20250928223921")]
  Muaat,
  [Description("Emirates of Hacaan")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/f/f8/Hacan.png/revision/latest/scale-to-width-down/55?cb=20201104005408")]
  Hacan,
  [Description("Federation of Sol")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/0/01/Sol.png/revision/latest/scale-to-width-down/55?cb=20201104005426")]
  Sol,
  [Description("Ghosts of Creuss")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/7/7f/Ghosts.png/revision/latest/scale-to-width-down/55?cb=20201104005444")]
  Creuss,
  [Description("L1z1x Mindnet")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/e/ec/L1Z1X.png/revision/latest/scale-to-width-down/55?cb=20201104231507")]
  L1z1x,
  [Description("Mentac Coalition")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/3/3c/Mentak.png/revision/latest/scale-to-width-down/55?cb=20201104005517")]
  Mentac,
  [Description("Naalu collective")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/a/a7/Naalu.png/revision/latest/scale-to-width-down/55?cb=20201104005533")]
  Naalu,
  [Description("Nekro virus")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/2/22/Nekro.png/revision/latest/scale-to-width-down/55?cb=20201104005553")]
  Nekro,
  [Description("Sardakk Norr")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/0/08/SardakkSymbolSquare.png/revision/latest/scale-to-width-down/55?cb=20250928223926")]
  Sardak,
  [Description("Universities of Jol-nar")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/0/06/Jol-Nar.png/revision/latest/scale-to-width-down/55?cb=20201104005643")]
  JolNar,
  [Description("Winnu")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/c/cd/Winnu.png/revision/latest/scale-to-width-down/55?cb=20201104005702")]
  Winnu,
  [Description("Xxcha Kingdom")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/1/1a/Xxcha.png/revision/latest/scale-to-width-down/55?cb=20201104005721")]
  Xxcha,
  [Description("Yin Brotherhood")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/f/f6/Yin.png/revision/latest/scale-to-width-down/55?cb=20201104005738")]
  Yin,
  [Description("Yssaril Tribes")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/a/ac/Yssaril.png/revision/latest/scale-to-width-down/55?cb=20201104005757")]
  Yssaril,
  [Description("Argent Flight")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/1/13/ArgentFactionSymbol.png/revision/latest/scale-to-width-down/55?cb=20201103113416")]
  Argent,
  [Description("Empyrean")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/c/ca/EmpyreanFactionSymbol.png/revision/latest/scale-to-width-down/55?cb=20201103113437")]
  Empyrean,
  [Description("Mahact Gene-sorcerers")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/2/2f/MahactSymbolSquare.png/revision/latest/scale-to-width-down/55?cb=20250928223932")]
  Mahact,
  [Description("Naaz-Rokha Alliance")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/3/3b/NaazRokhaSymbolSquare.png/revision/latest/scale-to-width-down/55?cb=20250928224404")]
  NaazRhoka,
  [Description("Nomad")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/5/5e/NomadFactionSheet.png/revision/latest/scale-to-width-down/55?cb=20201104084557")]
  Nomad,
  [Description("Titans of UI")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/6/6d/UlFactionSymbol.png/revision/latest/scale-to-width-down/55?cb=20201103113547")]
  Ui,
  [Description("The Vuil'Raith Cabal")]
  [FactionLink("https://static.wikia.nocookie.net/twilight-imperium-4/images/0/04/CabalFactionSymbol.png/revision/latest/scale-to-width-down/55?cb=20201103113606")]
  VuilRaith
}
