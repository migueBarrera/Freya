namespace FreyaApi.Repository.Interfaces;

public interface IClinicRepository
{
    IQueryable<EmployeeTable> GetEmployeesByClinic(Guid id, string? searchArgument = null);

    Task<bool> DeleteClinic(Guid id);

    Task<IEnumerable<Guid>> GetAllClientsIdFromClinic(Guid id);

    ClinicTable? GetClinic(Guid id);

    Task<bool> Update(UpdateClinicRequest request);

    Task<bool> Create(ClinicTable clinic);

    IQueryable<ClientTable> GetClientWithClinicInformation(Guid id, string? searchArgument = null);

    Task<bool> AddEmployeeToClinic(Guid clinicId, string employeeEmail);

    Task<bool> RemoveEmployeeFromClinic(Guid clinicId, Guid employeeId);

    Task<IEnumerable<ClinicTable>> GetAllClinicFromEmployeeByRol(Guid id);

    IQueryable<ClinicTable> GetAllClinicsByCompanyId(Guid id, string? searchArgument);

    Task<ClinicTable?> GetClinicWithEmployeeAndClientData(Guid id);

    IQueryable<EmployeeTable> GetEmployeeForClinic(Guid id, string? searchArgument = null);

    IQueryable<ClientTable> GetClientsForClinic(Guid id, string? searchArgument = null);

    bool ClinicExist(Guid clinicId);

    Task<bool> RemoveClientFromClinic(Guid clinicId, Guid clientId);

    Task AddClientToClinic(ClinicClient relation);

    Task<bool> UpdateClientPlan(Guid? clinicId, Guid? clientId, Guid subscriptionPlanId);
    Task<ClinicClient?> GetClientClinicRelation(Guid clientId, Guid clinicId);
}
