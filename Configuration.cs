using System.Text.Json;

namespace AfwezigheidsApp
{
    public class LogLevelSettings
    {
        public string Default { get; set; } = "Debug";
    }

    public class LoggingConfig
    {
        public LogLevelSettings LogLevel { get; set; } = new();
    }

    public class AppConfig
    {
        public LoggingConfig Logging { get; set; } = new();

        public static AppConfig Load()
        {
            if (File.Exists("appsettings.json"))
            {
                string json = File.ReadAllText("appsettings.json");
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            return new AppConfig();
        }

        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText("appsettings.json", json);
        }
    }
}