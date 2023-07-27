using System.Windows.Data;

namespace Freya.Desktop.Core.Converters;

public class IsZeroValueToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var num = (int)value;
        var hasParameter = parameter != null;

        var response = num == 0 
            ? Visibility.Collapsed 
            : Visibility.Visible;

        return hasParameter? response : InverterVisibility(response);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private Visibility InverterVisibility(Visibility response)
    {
        if (response == Visibility.Collapsed)
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Collapsed;
        }
    }
}
