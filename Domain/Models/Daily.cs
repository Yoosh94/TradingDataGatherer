namespace Domain.Models;

public class Daily : Bar<Daily>
{
    public long Volume { get; set; }
    public long NumberOfTrades { get; set; }
    public bool IsGreen => Close >= Open;
    public bool IsRed => Close < Open;
    public double GapPercentage { get; set; }

    public double? GapBelowLow { get; set; }
    public double? GapAboveHigh { get; set; }

}