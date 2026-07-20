namespace TwilightImperiumRoller.Models
{
    public class ShipStats
    {
        public string Name { get; set; } = string.Empty;
        public int Dice { get; set; }
        public int Hit { get; set; }
        public List<Result> Results { get; set; } = new List<Result>();
        public string Path { get; set; } = string.Empty ;
    }

    public class Result
    {
        public int DisplayValue;
        public int? FinalValue = null;
    }
}


