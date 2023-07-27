using System.Windows.Data;

namespace Freya.Desktop.Core.Converters;

public class OneIfIsZeroConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue <= 0 ? 1 : intValue;
        }
        else
        {
            return 1;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
