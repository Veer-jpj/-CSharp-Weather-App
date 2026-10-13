namespace WeatherApp;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        EnvLoader.Load();               // read .env before anything needs the API key
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
