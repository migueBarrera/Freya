namespace UploaderWasabi;

public static class WasabiBucketFolderHelper
{
    public static string BuildFolder(params string[] paths)
    {
        var s = string.Empty;
        foreach (var path in paths)
        {
            if (!string.IsNullOrEmpty(s))
            {
                s += "/" + path;
            }
            else
            {
                s += path;
            }
        }

        return s;
    }
}
