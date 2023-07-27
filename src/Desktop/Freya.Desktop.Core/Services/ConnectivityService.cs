namespace Freya.Desktop.Core.Services;

public class ConnectivityService : IConnectivityService
{
    public bool IsThereInternet => NetworkInterface.GetIsNetworkAvailable() == true;
}
