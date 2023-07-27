using System.Collections;
using System.Windows.Data;

namespace Freya.Desktop.Core.Converters;

public class ListAnyItemToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var response = ListAnyItemToVisibility(value, parameter);
        return response;
    }

    private Visibility ListAnyItemToVisibility(object value, object? parameter)
    {
        var hasParameter = parameter != null;
        var response = Visibility.Collapsed;
        if (value != null)
        {
            var count = 0;
            if (value is IList items)
            {
                count = items.Count;
                
            }
            else if(value is int number)
            {
                count = number;
            }
            else if(value is IEnumerable itemsEnum)
            {
                count = itemsEnum.Cast<object>().Count();
            }
            else
            {
                throw new ArgumentException();
            }

            response = count == 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }

        return hasParameter ? response : InverterVisibility(response);
    }

    private Visibility InverterVisibility(Visibility response)
    {
        if(response == Visibility.Collapsed)
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
