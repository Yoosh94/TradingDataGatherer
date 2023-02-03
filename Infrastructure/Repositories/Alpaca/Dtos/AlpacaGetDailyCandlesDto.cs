using System.Text.Json.Serialization;

namespace Infrastructure.Repositories.Alpaca.Dtos;

public class AlpacaGetDailyCandlesDto
{
    [JsonPropertyName("bars")]
    public List<Bar> Bars { get; set; }
}