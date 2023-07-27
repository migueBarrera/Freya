namespace Freya.Framework;

internal class LogPathHelper
{
    internal static string LogFolderPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Freya\\Logs\\";
    internal static string LogFilePath => LogFolderPath + "log.txt";
}
