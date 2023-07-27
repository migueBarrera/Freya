namespace UploaderWasabi;

public static class BucketHelper
{
    public static string ToClientBucket(Guid clientId)
    {
        return $"client{clientId}";
    }
}
