namespace Freya.Mobile.Firebase;

public sealed class CrossFirebaseSettings
{
    public CrossFirebaseSettings(
            bool isAnalyticsEnabled = false)
    {
        IsAnalyticsEnabled = isAnalyticsEnabled;
    }

    public override string ToString()
    {
        return $"[{nameof(CrossFirebaseSettings)}: " +
               $"{nameof(IsAnalyticsEnabled)}={IsAnalyticsEnabled}]";
    }

    public bool IsAnalyticsEnabled { get; }
}
