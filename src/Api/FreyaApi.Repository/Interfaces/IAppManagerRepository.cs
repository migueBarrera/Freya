namespace FreyaApi.Repository.Interfaces;

public interface IAppManagerRepository
{
    AppManagerTable? GetAppManager(string email);

    Task Create(AppManagerTable appManager);
}
