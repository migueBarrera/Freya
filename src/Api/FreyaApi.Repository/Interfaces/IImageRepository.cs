namespace FreyaApi.Repository.Interfaces;

public interface IImageRepository
{
    Task<IEnumerable<UltrasoundTable>> GetImages(Guid clientId, Guid clinicId);

    bool ImageExist(Guid id);

    Task<UltrasoundTable?> GetUltrasound(Guid imageId);

    Task<Guid> AddUltrasounds(UltrasoundTable ultrasound);

    Task<bool> RemoveUltraSound(Guid imageId);

    Task<bool> RemoveUltraSounds(IEnumerable<Guid> imageId);
}
