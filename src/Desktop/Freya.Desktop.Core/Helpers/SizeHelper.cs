namespace Freya.Desktop.Core.Helpers
{
    public class SizeHelper
    {
        public static long ConvertBytesToMB(long size)
        {
            double bToGbFactor = 1d / 1024 / 1024;

            return (long)(size * bToGbFactor);
        }
    }
}
