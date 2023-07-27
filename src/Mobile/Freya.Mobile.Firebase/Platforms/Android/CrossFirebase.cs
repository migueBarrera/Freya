using Android.App;
using Android.OS;
using Firebase;
using Freya.Mobile.Firebase.Analytics;

namespace Freya.Mobile.Firebase;

public static class CrossFirebase
{
    public static void Initialize(
        Activity activity,
        Bundle savedInstanceState,
        CrossFirebaseSettings settings,
        FirebaseOptions firebaseOptions = null,
        string name = null)
    {
        if (firebaseOptions == null)
        {
            var a = FirebaseApp.InitializeApp(activity);
        }
        else if (name == null)
        {
            FirebaseApp.InitializeApp(activity, firebaseOptions);
        }
        else
        {
            FirebaseApp.InitializeApp(activity, firebaseOptions, name);
        }

        if (settings.IsAnalyticsEnabled)
        {
            FirebaseAnalyticsImplementation.Initialize(activity);
        }


        Console.WriteLine($"Plugin.Firebase initialized with the following settings:\n{settings}");
    }
}
