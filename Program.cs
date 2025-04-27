namespace AfwezigheidsApp;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        using var loginFormulier = new LoginFormulier();
        Application.Run(loginFormulier);
    }    
}