using System.Windows.Data;

namespace Freya.Desktop.Core.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = value == null ? Visibility.Collapsed : Visibility.Visible;
            if (parameter != null)
            {
                visibility = visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
