using System.Windows.Data;
namespace Freya.Desktop.Core.Converters;

public class StringToPhoneConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return string.Empty;

        //retrieve only numbers in case we are dealing with already formatted phone no
        string phoneNo = value.ToString()!.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);

        return phoneNo.Length switch
        {
            4 => Regex.Replace(phoneNo, @"(\d{2})(\d{2})", "$1 $2"),// Ejemplo: 23-45
            5 => Regex.Replace(phoneNo, @"(\d{2})(\d{3})", "$1 $2"),// Ejemplo: 23-456
            6 => Regex.Replace(phoneNo, @"(\d{3})(\d{3})", "$1 $2"),// Ejemplo: 123-456
            7 => Regex.Replace(phoneNo, @"(\d{3})(\d{4})", "$1 $2"),// Ejemplo: 123-4567
            8 => Regex.Replace(phoneNo, @"(\d{4})(\d{4})", "$1 $2"),// Ejemplo: 1234-5678
            9 => Regex.Replace(phoneNo, @"(\d{3})(\d{3})(\d{3})", "$1 $2 $3"),
            10 => Regex.Replace(phoneNo, @"(\d{3})(\d{2})(\d{2})(\d{3})", "$1 $2 $3 $4"),
            _ => phoneNo,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
