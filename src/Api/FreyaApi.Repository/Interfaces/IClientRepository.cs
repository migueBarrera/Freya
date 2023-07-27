using Models.Core.Clients;

namespace FreyaApi.Repository.Interfaces;

public interface IClientRepository
{
    Task<ClientTable?> GetClientByEmail(string email);

    bool ClientExist(string email);

    bool ClientExist(Guid id);

    Task<IEnumerable<ClinicTable>> GetClinicsForClient(Guid id);

    bool ClienHasIncludeOnAClinic(string clientEmail, Guid clinicId);

    Task<bool> UpdatePass(string email, string pass);

    Task<bool> UpdatePass(Guid clientId, string pass);

    ClientTable? GetClientById(Guid clientId);

    ClientTable? GetClientByIdWithClinicRelation(Guid clientId, Guid clinicId);

    Task<ClientTable?> Create(ClientTable client);

    Task<bool> UpdateClient(Guid id, ClientUpdateRequest request);
    Task<Guid?> GetClientIdByEmail(string clientEmail);

    Task<long> GetSizeFromClientAndClinic(Guid clientId, Guid clinicId);
}
