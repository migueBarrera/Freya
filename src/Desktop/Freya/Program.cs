using System.Diagnostics;

namespace Freya;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var splashBackground = new System.Windows.SplashScreen("Resources/Splash_Screen.png");
        splashBackground.Show(false, !Debugger.IsAttached);

        var application = new App();
        application.InitializeComponent();
        application.Loaded += (object? sender, EventArgs e) =>
        {
            splashBackground.Close(TimeSpan.FromMilliseconds(300));
        };
        application.Run();
    }
}
