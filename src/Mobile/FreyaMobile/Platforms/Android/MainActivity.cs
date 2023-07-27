using Android.App;
using Android.Content.PM;
using Android.OS;

namespace FreyaMobile;

[Activity(
    Theme = "@style/Maui.SplashTheme", 
    MainLauncher = true, 
    WindowSoftInputMode = Android.Views.SoftInput.StateAlwaysHidden | Android.Views.SoftInput.AdjustPan,
    ConfigurationChanges = ConfigChanges.ScreenSize 
        | ConfigChanges.Orientation 
        | ConfigChanges.UiMode 
        | ConfigChanges.ScreenLayout 
        | ConfigChanges.SmallestScreenSize 
        | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }
}