using Domain.Models;

namespace Application.Services;

public interface IRuleEngine
{
    Task Activate(string symbol, DateTime from, DateTime to);
    List<Daily> Execute();

    IRuleEngine OpenOutsideOfBody();
    IRuleEngine OpenBelowYesterdayLow();

    IRuleEngine OpenAboveYesterdayHigh();
    IRuleEngine YesterdayClosedGreen();
    IRuleEngine YesterdayClosedRed();
    IRuleEngine AllCandles();
}