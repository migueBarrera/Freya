namespace FreyaMobile.Core.Framework;

public class LogPathHelper
{
    private const string LogFolderName = "logs";
    private const string LogTemplateFilename = "log.txt";

    public static string LogFolderPath => Path.Combine(FileSystem.AppDataDirectory, LogFolderName);
}
