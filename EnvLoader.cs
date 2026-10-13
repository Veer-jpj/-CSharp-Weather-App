namespace WeatherApp;

/// <summary>Minimal .env loader: KEY=VALUE lines, # comments, optional quotes.</summary>
public static class EnvLoader
{
    public static void Load()
    {
        var path = FindEnvFile();
        if (path == null) return;

        foreach (var raw in File.ReadAllLines(path))
        {
            var line = raw.Trim();
            if (line.Length == 0 || line.StartsWith('#')) continue;

            var idx = line.IndexOf('=');
            if (idx <= 0) continue;

            var key = line[..idx].Trim();
            var value = line[(idx + 1)..].Trim().Trim('"', '\'');
            Environment.SetEnvironmentVariable(key, value);
        }
    }

    // Looks in the exe folder, then walks up parent folders
    // (so it finds the project-root .env when running from bin/Debug/...).
    private static string? FindEnvFile()
    {
        foreach (var start in new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() })
        {
            var dir = new DirectoryInfo(start);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, ".env");
                if (File.Exists(candidate)) return candidate;
                dir = dir.Parent;
            }
        }
        return null;
    }
}
