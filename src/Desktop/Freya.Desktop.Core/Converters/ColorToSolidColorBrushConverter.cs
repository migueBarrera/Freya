using System.Windows.Data;
using System.Windows.Media;

namespace Freya.Desktop.Core.Converters
{
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush();
            }

            if (value is Color)
            {
                Color color = (Color)value;
                return new SolidColorBrush(color);
            }

            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}