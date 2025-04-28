using System;
using System.IO;

namespace AfwezigheidsApp
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public static class Logger
    {
        private static LogLevel _minimumLogLevel = LogLevel.Debug;
        private static readonly string LogFile = "debug.log";
        
        public static LogLevel MinimumLogLevel 
        { 
            get => _minimumLogLevel;
            set
            {
                _minimumLogLevel = value;
                Debug($"Log level changed to {value}");
            }
        }

        public static void SetLogLevelFromString(string level)
        {
            if (Enum.TryParse<LogLevel>(level, true, out var logLevel))
            {
                MinimumLogLevel = logLevel;
            }
            else
            {
                Warning($"Invalid log level: {level}. Using current level: {MinimumLogLevel}");
            }
        }

        public static void Log(LogLevel level, string message)
        {
            if (level >= MinimumLogLevel)
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string logMessage = $"[{timestamp}] [{level}] {message}";
                
                Console.WriteLine(logMessage);
                System.Diagnostics.Debug.WriteLine(logMessage);
                
                try
                {
                    File.AppendAllText(LogFile, logMessage + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write to log file: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Failed to write to log file: {ex.Message}");
                }
            }
        }

        public static void Debug(string message) => Log(LogLevel.Debug, message);
        public static void Info(string message) => Log(LogLevel.Info, message);
        public static void Warning(string message) => Log(LogLevel.Warning, message);
        public static void Error(string message) => Log(LogLevel.Error, message);
    }
}