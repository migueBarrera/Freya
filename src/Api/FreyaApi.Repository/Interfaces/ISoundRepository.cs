namespace FreyaApi.Repository.Interfaces;

public interface ISoundRepository
{
    Task<IEnumerable<SoundTable>> GetSounds(Guid clientId, Guid clinicId);

    bool SoundExist(Guid id);

    Task<SoundTable?> GetSound(Guid soundId);

    Task<Guid> AddSound(SoundTable sound);

    Task<bool> RemoveSounds(IEnumerable<Guid> soundId);

    Task<bool> RemoveSound(Guid soundId);
}
