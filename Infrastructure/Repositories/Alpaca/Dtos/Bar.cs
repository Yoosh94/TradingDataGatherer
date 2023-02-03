using System.Text.Json.Serialization;

namespace Infrastructure.Repositories.Alpaca.Dtos;

public class Bar
{
    [JsonPropertyName("t")] public DateTime T { get; set; }
    [JsonPropertyName("o")] public double O { get; set; }
    [JsonPropertyName("c")] public double C { get; set; }
    [JsonPropertyName("h")] public double H { get; set; }
    [JsonPropertyName("l")] public double L { get; set; }
    [JsonPropertyName("v")] public long V { get; set; }
    [JsonPropertyName("n")] public long N { get; set; }
}