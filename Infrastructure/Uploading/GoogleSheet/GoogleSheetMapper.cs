using System.Collections.Immutable;
using System.Globalization;
using Domain.Models;

namespace Infrastructure.Uploading.GoogleSheet;

public static class GoogleSheetMapper
{
    public static IList<IList<object>> MapToRangeData(IEnumerable<Daily> dailyBars)
    {
        var objectBars = dailyBars.Select((daily, index) => new List<object>()
        {
            daily.Date.Date.ToString("m",CultureInfo.GetCultureInfo("en-AU")),
            daily.Open,
            daily.Close,
            daily.High,
            daily.Low,
            daily.Volume,
            daily.NumberOfTrades,
            daily.GapPercentage,
            daily.IsGreen,
            daily.IsRed,
        } as IList<object>).ToList();
        objectBars.Insert(0,new List<object>()
        {
            nameof(Daily.Date),
            nameof(Daily.Open),
            nameof(Daily.Close),
            nameof(Daily.High),
            nameof(Daily.Low),
            nameof(Daily.Volume),
            nameof(Daily.NumberOfTrades),
            nameof(Daily.GapPercentage),
            nameof(Daily.IsGreen),
            nameof(Daily.IsRed),
        });
        return objectBars;
    }
    
    // Use this method when you want to get data for your playbook
    public static IList<IList<object>> MapToPlayBookData(IEnumerable<Daily> dailyBars)
    {
        var objectBars = dailyBars.Select((daily, index) => new List<object>()
        {
            daily.Date.Date.ToString("d",CultureInfo.GetCultureInfo("en-AU")),
            null,
            daily.GapPercentage,
            daily.GapAboveHigh,
            daily.GapBelowLow
        } as IList<object>).ToList();
        return objectBars;
    }
}