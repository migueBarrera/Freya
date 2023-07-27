namespace FreyaApi.Repository.Services;

public class AppManagerRepository : BaseRepository, IAppManagerRepository
{
    public AppManagerRepository(DatabaseContext databaseContext)
        : base(databaseContext)
    {
    }

    public async Task Create(AppManagerTable appManager)
    {
        context.AppManager?.Add(appManager);
        await context.SaveChangesAsync();
    }

    public AppManagerTable? GetAppManager(string email)
    {
        return context.AppManager?
       .Where(a => a.Email == email)
       .AsNoTracking()
       .FirstOrDefault()!;
    }
}
