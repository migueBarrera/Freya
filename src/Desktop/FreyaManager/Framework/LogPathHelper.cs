namespace FreyaManager.Framework;

internal class LogPathHelper
{
    internal static string LogFolderPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FreyaManager\\Logs\\";
    internal static string LogFilePath => LogFolderPath + "log.txt";
}
