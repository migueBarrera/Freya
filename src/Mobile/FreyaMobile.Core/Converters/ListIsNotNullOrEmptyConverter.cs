namespace FreyaMobile.Core.Converters;

public class ListIsNotNullOrEmptyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var result = new ListIsNullOrEmptyConverter().Convert(value, targetType, parameter, culture);
        return !(bool)result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
