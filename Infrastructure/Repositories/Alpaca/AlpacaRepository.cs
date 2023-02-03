using System.Text.Json;
using System.Xml;
using Application.Services;
using Domain.Models;
using Infrastructure.Repositories.Alpaca.Dtos;

namespace Infrastructure.Repositories.Alpaca;

public class AlpacaRepository : IRepository
{
    private readonly HttpClient _client;


    public AlpacaRepository(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Daily>> GetDailyBarsAsync(string symbol, DateTime @from, DateTime to)
    {
        try
        {
            var fromFormatted = XmlConvert.ToString(from, XmlDateTimeSerializationMode.Utc);
            var toFormatted = XmlConvert.ToString(to, XmlDateTimeSerializationMode.Utc);
            ///var response = await _client.GetFromJsonAsync<AlpacaGetDailyCandlesDto>($"v2/stocks/{symbol}/bars?timeframe=1Day&start={fromFormatted}&end={toFormatted}");
            var response =
                await _client.GetAsync(
                    $"v2/stocks/{symbol}/bars?timeframe=1Day&start={fromFormatted}&end={toFormatted}");
            var responseAsString = await response.Content.ReadAsStringAsync();
            var d = JsonSerializer.Deserialize<AlpacaGetDailyCandlesDto>(responseAsString);
            var dailyCandles = MapToDomainModels(d!);
            return dailyCandles;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return new List<Daily>();
    }

    private static List<Daily> MapToDomainModels(AlpacaGetDailyCandlesDto dailyDtos)
    {
        var domainModel = new List<Daily>();
        for (var i = 1; i < dailyDtos.Bars.Count; i++)
        {
            var today = new Daily()
            {
                Close = dailyDtos.Bars[i].C,
                High = dailyDtos.Bars[i].H,
                Low = dailyDtos.Bars[i].L,
                Open = dailyDtos.Bars[i].O,
                Volume = dailyDtos.Bars[i].V,
                NumberOfTrades = dailyDtos.Bars[i].N,
                Date = dailyDtos.Bars[i].T,
                GapPercentage = (dailyDtos.Bars[i].O - dailyDtos.Bars[i - 1].C) / dailyDtos.Bars[i - 1].C,
                GapBelowLow = CalculateGapBelowLowPercentage(dailyDtos.Bars[i-1], dailyDtos.Bars[i]),
                GapAboveHigh = CalculateGapAboveHighPercentage(dailyDtos.Bars[i-1], dailyDtos.Bars[i]),
                Previous = new Daily(){
                    Close = dailyDtos.Bars[i-1].C,
                    High = dailyDtos.Bars[i-1].H,
                    Low = dailyDtos.Bars[i-1].L,
                    Open = dailyDtos.Bars[i-1].O,
                    Volume = dailyDtos.Bars[i-1].V,
                    NumberOfTrades = dailyDtos.Bars[i-1].N,
                    Date = dailyDtos.Bars[i-1].T,
                }
            };
            domainModel.Add(today);
            
        }
        return domainModel;
    }

    private static double? CalculateGapBelowLowPercentage(Alpaca.Dtos.Bar yesterday, Alpaca.Dtos.Bar today)
    {
        if (today.O < yesterday.L)
        {
            return (today.O - yesterday.L) / yesterday.L;
        }

        return null;
    }
    
    private static double? CalculateGapAboveHighPercentage(Alpaca.Dtos.Bar yesterday, Alpaca.Dtos.Bar today)
    {
        if (today.O > yesterday.H)
        {
            return (today.O - yesterday.H) / yesterday.H;
        }

        return null;
    }
}