using System.Net;
using System.Text.Json;

namespace WeatherApp;

public class WeatherService
{
    private const string BaseUrl =
        "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline";

    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(20) };
    private readonly string _apiKey;

    public WeatherService(string apiKey) => _apiKey = apiKey;

    /// <param name="unitGroup">"metric" or "us"</param>
    public async Task<WeatherResponse> GetWeatherAsync(
        string location, string unitGroup, CancellationToken ct = default)
    {
        var url = $"{BaseUrl}/{Uri.EscapeDataString(location)}/next7days" +
                  $"?unitGroup={unitGroup}&include=current%2Cdays&contentType=json" +
                  $"&key={Uri.EscapeDataString(_apiKey)}";

        using var response = await Http.GetAsync(url, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            // Don't include the URL in messages: it contains the API key.
            throw new HttpRequestException(response.StatusCode switch
            {
                HttpStatusCode.BadRequest => $"Location not found or invalid request: {body}",
                HttpStatusCode.Unauthorized => "Invalid API key.",
                HttpStatusCode.TooManyRequests => "API rate/usage limit reached.",
                _ => $"Weather service error ({(int)response.StatusCode}): {body}"
            });
        }

        return JsonSerializer.Deserialize<WeatherResponse>(body)
               ?? throw new InvalidOperationException("Empty response from weather service.");
    }
}
