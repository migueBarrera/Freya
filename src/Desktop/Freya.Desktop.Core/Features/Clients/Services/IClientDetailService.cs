using Models.Core.Clients;

namespace Freya.Desktop.Core.Features.Clients.Services;

public interface IClientDetailService
{
    Task<ClientDetailResponse> GetClientDetails(Guid clientId, Guid clinicId);

    Task<bool> DeleteClient(Guid clientId, Guid clinicId);

    Task<bool> UpdateClientPlan(Guid id, Guid clinicId);
}
