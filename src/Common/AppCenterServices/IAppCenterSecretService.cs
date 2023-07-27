namespace AppCenterServices;

public interface IAppCenterSecretService
{
    bool IsEnabledAnalitics { get; }

    bool IsEnabledCrashes { get; }

    string GetSecret();
}
