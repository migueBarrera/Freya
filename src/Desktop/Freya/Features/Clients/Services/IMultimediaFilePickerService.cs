namespace Freya.Features.Clients.Services;

public interface IMultimediaFilePickerService
{
    Task<bool> OpenFilePickerAndUploadMultimedia(
        string filter,
        MultimediaType multimediaType,
        Guid clientId,
        Guid clinicId,
        string clinicName,
        string clientName,
        long limitSize,
        long spaceUsedByClient);
}
