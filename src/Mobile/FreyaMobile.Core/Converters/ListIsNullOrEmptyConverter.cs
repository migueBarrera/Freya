namespace FreyaMobile.Core.Converters;

internal class ListIsNullOrEmptyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return true;

        if (!(value is IEnumerable))
        {
            throw new ArgumentException($"{nameof(ListIsNullOrEmptyConverter)} Value must be a IEnumerable");
        }

        var castedList = (IEnumerable)value;

        return !castedList.GetEnumerator().MoveNext();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
