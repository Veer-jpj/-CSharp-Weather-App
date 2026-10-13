# WeatherApp

Windows Forms (.NET 8) app that shows current conditions and a 7-day forecast
using the [Visual Crossing Weather API](https://www.visualcrossing.com/weather-api).

## Setup
1. Copy `.env.example` to `.env` and put your key in it:
   ```
   VISUAL_CROSSING_API_KEY=your_api_key_here
   ```
2. `.env` is listed in `.gitignore`, so it won't be committed.
3. Run:
   ```
   dotnet run
   ```
   (or open the folder in Visual Studio 2022 and press F5)

Requires Windows and the .NET 8 SDK.
