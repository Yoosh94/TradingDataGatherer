using Domain.Models;

namespace Application.Services;

public class RulesEngine : IRuleEngine
{
    private readonly IRepository _repository;
    private readonly ICandleFilteringService _candleFilteringService;
    private string Symbol { get; set; }
    private  DateTime From { get; set; }
    private  DateTime To { get; set; }

    private List<Daily> _dailyCandles;

    
    public RulesEngine(IRepository repository, ICandleFilteringService candleFilteringService)
    {
        _repository = repository;
        _candleFilteringService = candleFilteringService;
    }

    public async Task Activate(string symbol, DateTime @from, DateTime to)
    {
        Symbol = symbol;
        From = @from;
        To = to;
        _dailyCandles = await _repository.GetDailyBarsAsync(Symbol, From, To);
    }

    public List<Daily> Execute()
    {
        return _dailyCandles;
    }

    public IRuleEngine OpenOutsideOfBody()
    {
        var filtered = _candleFilteringService.RemoveInsideBodyDays(_dailyCandles);
        Console.WriteLine(filtered.Count);
        _dailyCandles = filtered;
        return this;
    }

    public IRuleEngine OpenBelowYesterdayLow()
    {
        var filtered = _candleFilteringService.SelectOpenBelowLow(_dailyCandles);
        Console.WriteLine(filtered.Count);
        _dailyCandles = filtered;
        return this;
    }
    
    public IRuleEngine OpenAboveYesterdayHigh()
    {
        var filtered = _candleFilteringService.SelectOpenAboveHigh(_dailyCandles);
        Console.WriteLine(filtered.Count);
        _dailyCandles = filtered;
        return this;
    }

    public IRuleEngine YesterdayClosedGreen()
    {
        var filtered = _candleFilteringService.SelectYesterdayIsGreen(_dailyCandles);
        Console.WriteLine(filtered.Count);
        _dailyCandles = filtered;
        return this;
    }
    
    public IRuleEngine YesterdayClosedRed()
    {
        var filtered = _candleFilteringService.SelectYesterdayIsRed(_dailyCandles);
        Console.WriteLine(filtered.Count);
        _dailyCandles = filtered;
        return this;
    }

    
    public IRuleEngine AllCandles()
    {
        // No filtering needed since we want all the candles
        return this;
    }
}