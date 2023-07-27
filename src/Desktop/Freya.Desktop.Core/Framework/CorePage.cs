using System.Windows.Controls;

namespace Freya.Desktop.Core.Framework
{
    public class CorePage : Page
    {
        public CorePage()
        {
            this.Loaded += PageLoaded;
            this.Unloaded += PageUnLoaded;
        }

        private CoreViewModel? ViewModel => DataContext as CoreViewModel;

        protected virtual void PageUnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.OnDisappearingAsync();
        }

        protected virtual void PageLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.OnAppearingAsync();
        }
    }
}
