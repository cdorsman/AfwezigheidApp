using System.Text;
using System.Globalization;
using System.Threading;

namespace AfwezigheidsApp;

static class Program
{
    public static AppConfig Config { get; private set; } = new();

    [STAThread]
    static void Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-NL");
        
        Config = AppConfig.Load();
        Logger.SetLogLevelFromString(Config.Logging.LogLevel.Default);
        
        ApplicationConfiguration.Initialize();
        using var loginFormulier = new LoginFormulier();
        Application.Run(loginFormulier);
    }    
}