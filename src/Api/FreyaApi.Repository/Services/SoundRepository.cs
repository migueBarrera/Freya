namespace FreyaApi.Repository.Services;

public class SoundRepository : BaseRepository, ISoundRepository
{
    public SoundRepository(DatabaseContext databaseContext)
         : base(databaseContext)
    {
    }

    public async Task<IEnumerable<SoundTable>> GetSounds(Guid id, Guid clinicId)
    {
        var videos = await context.Sounds?
            .Where(x => x.ClientId == id && x.ClinicId == clinicId)
            .OrderByDescending(x => x.Created)
            .AsNoTracking()
            .ToListAsync()!;

        if (videos == null)
        {
            return Enumerable.Empty<SoundTable>();
        }

        return videos;
    }

    public bool SoundExist(Guid id)
    {
        return context.Sounds?.Any(x => x.Id == id) ?? false;
    }

    public async Task<SoundTable?> GetSound(Guid soundId)
    {
        return await context.Sounds?
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == soundId)!;
    }

    public async Task<Guid> AddSound(SoundTable sound)
    {
        context.Sounds?.Add(sound);
        await context.SaveChangesAsync();
        return sound.Id;
    }

    public async Task<bool> RemoveSound(Guid soundId)
    {
        var item = context.Sounds?
        .FirstOrDefault(x => x.Id == soundId);
        if (item == null)
        {
            return false;
        }

        context.Sounds?.Remove(item);

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveSounds(IEnumerable<Guid> soundId)
    {
        var items = context.Sounds?
            .Where(x => soundId.Contains(x.Id))
            .ToList();

        bool hasAnyItem = items?.Any() ?? false;
        if (!hasAnyItem)
        {
            return false;
        }

        context.Sounds?.RemoveRange(items!);

        await context.SaveChangesAsync();


        return true;
    }
}
