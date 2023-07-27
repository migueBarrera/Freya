using System.Windows.Data;

namespace Freya.Desktop.Core.Converters
{
    public class SizeToTextConverter : IValueConverter
    {
        static string[] sizes = { "B", "KB", "MB", "GB", "TB" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertToText(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static string ConvertToText(object value)
        {
            long len = (long)value;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
