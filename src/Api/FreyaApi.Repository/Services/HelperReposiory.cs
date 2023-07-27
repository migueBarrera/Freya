namespace FreyaApi.Repository.Services;

public class HelperReposiory : BaseRepository, IHelperReposiory
{
    public HelperReposiory(DatabaseContext databaseContext)
         : base(databaseContext)
    {
    }

    public async Task<bool> RecreateDatabase(bool delete)
    {
        if (delete)
        {
            await context.Database.EnsureDeletedAsync();
        }
        return await context.Database.EnsureCreatedAsync();
    }
}
