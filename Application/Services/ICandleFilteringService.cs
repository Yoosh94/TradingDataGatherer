using Domain.Models;

namespace Application.Services;

public interface ICandleFilteringService
{
    List<Daily> RemoveInsideBodyDays(List<Daily> dailyCandles);
    List<Daily> SelectOpenBelowLow(List<Daily> dailyCandles);
    List<Daily> SelectOpenAboveHigh(List<Daily> dailyCandles);
    List<Daily> SelectYesterdayIsGreen(List<Daily> dailyCandles);
    List<Daily> SelectYesterdayIsRed(List<Daily> dailyCandles);

}