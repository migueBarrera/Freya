namespace FreyaMobile.Core.Services;

public class ConnectivityService : IConnectivityService
{
    public bool IsThereInternet => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
}
