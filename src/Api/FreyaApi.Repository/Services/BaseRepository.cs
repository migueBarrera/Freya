namespace FreyaApi.Repository.Services;

public abstract class BaseRepository : IDisposable
{
    protected DatabaseContext context;
    private bool disposed = false;

    public BaseRepository(DatabaseContext context)
    {
        this.context = context;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
