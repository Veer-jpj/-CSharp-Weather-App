using System.Text.Json.Serialization;

namespace WeatherApp;

public class WeatherResponse
{
    [JsonPropertyName("resolvedAddress")] public string? ResolvedAddress { get; set; }
    [JsonPropertyName("timezone")] public string? Timezone { get; set; }
    [JsonPropertyName("currentConditions")] public CurrentConditions? Current { get; set; }
    [JsonPropertyName("days")] public List<DayForecast> Days { get; set; } = new();
}

public class CurrentConditions
{
    [JsonPropertyName("temp")] public double? Temp { get; set; }
    [JsonPropertyName("feelslike")] public double? FeelsLike { get; set; }
    [JsonPropertyName("humidity")] public double? Humidity { get; set; }
    [JsonPropertyName("windspeed")] public double? WindSpeed { get; set; }
    [JsonPropertyName("pressure")] public double? Pressure { get; set; }
    [JsonPropertyName("visibility")] public double? Visibility { get; set; }
    [JsonPropertyName("uvindex")] public double? UvIndex { get; set; }
    [JsonPropertyName("conditions")] public string? Conditions { get; set; }
    [JsonPropertyName("sunrise")] public string? Sunrise { get; set; }
    [JsonPropertyName("sunset")] public string? Sunset { get; set; }
}

public class DayForecast
{
    [JsonPropertyName("datetime")] public string? Date { get; set; }
    [JsonPropertyName("tempmax")] public double? TempMax { get; set; }
    [JsonPropertyName("tempmin")] public double? TempMin { get; set; }
    [JsonPropertyName("precipprob")] public double? PrecipProb { get; set; }
    [JsonPropertyName("conditions")] public string? Conditions { get; set; }
}
