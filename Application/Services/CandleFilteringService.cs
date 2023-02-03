using Domain.Models;

namespace Application.Services;

public class CandleFilteringService : ICandleFilteringService
{
    public List<Daily> RemoveInsideBodyDays(List<Daily> dailyCandles)
    {
        var dailyWithoutInsideDays = new List<Daily>();
        for (int i = 1; i < dailyCandles.Count; i++)
        {
            var yesterday = dailyCandles[i - 1];
            var today = dailyCandles[i];
            if (yesterday.IsGreen && (today.Open > yesterday.Close || today.Open < yesterday.Open))
            {
                dailyWithoutInsideDays.Add(dailyCandles[i]);
            }
            else if(yesterday.IsRed && (today.Open > yesterday.Open || today.Open < yesterday.Close))
            {
                dailyWithoutInsideDays.Add(dailyCandles[i]);
            }
        }
        return dailyWithoutInsideDays;
    }

    public List<Daily> SelectOpenBelowLow(List<Daily> dailyCandles)
    {
        var dailyOpeningBelowYesterday = new List<Daily>();
        for (int i = 1; i < dailyCandles.Count; i++)
        {
            var yesterday = dailyCandles[i - 1];
            var today = dailyCandles[i];
            if (today.Open < yesterday.Low)
            {
                dailyOpeningBelowYesterday.Add(today);
            }
        }

        return dailyOpeningBelowYesterday;
    }
    
    public List<Daily> SelectOpenAboveHigh(List<Daily> dailyCandles)
    {
        var dailyOpeningAboveYesterday = new List<Daily>();
        for (int i = 1; i < dailyCandles.Count; i++)
        {
            var yesterday = dailyCandles[i - 1];
            var today = dailyCandles[i];
            if (today.Open > yesterday.High)
            {
                dailyOpeningAboveYesterday.Add(today);
            }
        }

        return dailyOpeningAboveYesterday;
    }

    public List<Daily> SelectYesterdayIsGreen(List<Daily> dailyCandles)
    {
        var dailyCandlesWhereYesterdayIsGreen = new List<Daily>();
        for (int i = 0; i < dailyCandles.Count; i++)
        {
            if (dailyCandles[i].Previous.IsGreen)
            {
                dailyCandlesWhereYesterdayIsGreen.Add(dailyCandles[i]);
            }
        }
        return dailyCandlesWhereYesterdayIsGreen;
    }
    
    public List<Daily> SelectYesterdayIsRed(List<Daily> dailyCandles)
    {
        var dailyCandlesWhereYesterdayIsRed = new List<Daily>();
        for (int i = 0; i < dailyCandles.Count; i++)
        {
            if (dailyCandles[i].Previous.IsRed)
            {
                dailyCandlesWhereYesterdayIsRed.Add(dailyCandles[i]);
            }
        }
        return dailyCandlesWhereYesterdayIsRed;
    }
}