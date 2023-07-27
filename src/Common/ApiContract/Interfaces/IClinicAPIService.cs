using Models.Core.Clients;
using Models.Core.Clinics;

namespace ApiContract.Interfaces;

public interface IClinicAPIService
{
    [Post("/api/Clinic/v1")]
    Task<ClinicCreateResponse> CreateAsync([Body] ClinicCreateRequest request);
    
    [Put("/api/Clinic/v1")]
    Task Update([Body] UpdateClinicRequest request);
    
    [Get("/api/Clinic/v1/{id}")]
    Task<ClinicDetailResponse> GetAsync(Guid id);
    
    [Delete("/api/Clinic/v1/{id}")]
    Task Delete(Guid id);

    [Get("/api/Clinic/v1/{clinicId}/clients")]
    Task<PagedModel<ClientResponse>> GetClientsByClinicAsync(Guid clinicId, [Query] PaginationFilter PaginationFilter);
    
    [Get("/api/companies/v1/{companyId}/clinics")]
    Task<PagedModel<ClinicResponse>> GetClinicsByCompanyIdAsync(Guid companyId, [Query] PaginationFilter PaginationFilter);
}
