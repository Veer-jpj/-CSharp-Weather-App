# 🌦️ WeatherApp

A Windows Forms desktop application built with **C# and .NET 8** that displays current weather conditions and a 7-day weather forecast using the [Visual Crossing Weather API](https://www.visualcrossing.com/weather-api).

## ✨ Features

- Search weather by city.
- View current weather conditions.
- Get a 7-day weather forecast.
- Display weather details in a Windows desktop interface.
- Store API credentials securely in a local `.env` file.

## 🛠️ Technologies Used

- C#
- .NET 8 Windows Forms
- Visual Crossing Weather API
- Environment variables
- Git and GitHub

## ⚙️ Setup and Installation

### Prerequisites

- Windows
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A Visual Crossing Weather API key

### Steps

1. Clone this repository:

   ```bash
   git clone https://github.com/Veer-jpj/CSharp-Weather-App.git
   ```

2. Open the project folder:

   ```bash
   cd CSharp-Weather-App/WeatherApp
   ```

3. Copy `.env.example` to `.env` and add your API key:

   ```env
   VISUAL_CROSSING_API_KEY=your_api_key_here
   ```

4. Restore dependencies and run the application:

   ```bash
   dotnet restore
   dotnet run
   ```

   Alternatively, open `WeatherApp.csproj` in Visual Studio 2022 and press **F5**.

## 🔐 API Key Security

- The `.env` file is excluded using `.gitignore`.
- Never commit or publish your real API key.
- Use `.env.example` as a template without real credentials.

## 📁 Project Structure

```text
CSharp-Weather-App/
├── WeatherApp/
│   ├── Controls/
│   ├── Models/
│   ├── Services/
│   ├── Form1.cs
│   ├── Program.cs
│   ├── WeatherApp.csproj
│   ├── .env.example
│   └── .gitignore
├── .gitignore
└── README.md
```

## 👨‍💻 Author

**Veer Prajapati**

Built as a C# .NET learning project.
