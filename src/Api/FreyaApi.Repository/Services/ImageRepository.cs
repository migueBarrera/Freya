namespace FreyaApi.Repository.Services;

public class ImageRepository : BaseRepository, IImageRepository
{
    public ImageRepository(DatabaseContext databaseContext)
         : base(databaseContext)
    {
    }

    public async Task<IEnumerable<UltrasoundTable>> GetImages(Guid id, Guid clinicId)
    {
        var ultrasounds = await context.Ultrasounds?
            .Where(x => x.ClientId == id && x.ClinicId == clinicId)
            .OrderByDescending(x => x.Created)
            .AsNoTracking()
            .ToListAsync()!;

        if (ultrasounds == null)
        {
            return Enumerable.Empty<UltrasoundTable>();
        }

        return ultrasounds;
    }

    public async Task<UltrasoundTable?> GetUltrasound(Guid imageId)
    {
        var item = await context
        .Ultrasounds?
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == imageId)!;

        return item;
    }

    public bool ImageExist(Guid id)
    {
        return context.Ultrasounds?.Any(x => x.Id == id) ?? false;
    }

    public async Task<Guid> AddUltrasounds(UltrasoundTable ultrasound)
    {
        context.Ultrasounds?.Add(ultrasound);
        await context.SaveChangesAsync();
        return ultrasound.Id;
    }

    public async Task<bool> RemoveUltraSound(Guid imageId)
    {
        var item = context.Ultrasounds?.FirstOrDefault(x => x.Id == imageId);
        if (item == null)
        {
            return false;
        }

        context.Ultrasounds?.Remove(item);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveUltraSounds(IEnumerable<Guid> imageId)
    {
        var items = context.Ultrasounds?
               .Where(x => imageId.Contains(x.Id))
               .ToList();

        bool hasAnyItem = items?.Any() ?? false;
        if (!hasAnyItem)
        {
            return false;
        }

        context.Ultrasounds?.RemoveRange(items!);

        await context.SaveChangesAsync();

        return true;
    }
}
