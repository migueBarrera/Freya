namespace UploaderWasabi;

public class UploadObjectResponse
{
    public bool Success { get; set; } = false;

    public string Error { get; set; } = string.Empty;

    public Uri? Uri { get; set; }
}
