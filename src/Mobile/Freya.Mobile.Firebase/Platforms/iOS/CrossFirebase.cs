using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Freya.Mobile.Firebase
{
    public static class CrossFirebase
    {
        public static void Initialize(
            UIApplication app,
            NSDictionary options,
            CrossFirebaseSettings settings,
            object firebaseOptions = null,
            string name = null)
        {
            //TODO
            //if (firebaseOptions == null)
            //{
            //    App.Configure();
            //}
            //else if (name == null)
            //{
            //    App.Configure(firebaseOptions);
            //}
            //else
            //{
            //    App.Configure(name, firebaseOptions);
            //}

            //if (settings.IsAnalyticsEnabled)
            //{
            //    FirebaseAnalyticsImplementation.Initialize();
            //}


            Console.WriteLine($"Plugin.Firebase initialized with the following settings:\n{settings}");
        }
    }
}
