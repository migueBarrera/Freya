namespace Freya.Features.Clients.Services;

public interface ICheckClientService
{
    Task<Result<NewClientState>> CheckNewClientState(string email, Guid clinicId);

    Task<Result<bool>> AssignClientToSelectedClinic(string email, Guid clinicId);
}
