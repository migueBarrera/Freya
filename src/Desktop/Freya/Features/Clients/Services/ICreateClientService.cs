namespace Freya.Features.Clients.Services;

public interface ICreateClientService
{
    Task<bool> CreateClient(Client client, Guid clinicId);
}
