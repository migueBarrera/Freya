using System.Windows.Data;

namespace Freya.Desktop.Core.Converters;

public class CentsToEurConverters : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            var amount = (long)value;
            var x = (double)amount;
            var cents = x / 100;

            return cents;
        }
        catch (Exception)
        {
            return "Invalid";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
