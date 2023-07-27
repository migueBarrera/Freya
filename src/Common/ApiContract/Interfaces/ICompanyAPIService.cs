using Models.Core.Companies;

namespace ApiContract.Interfaces;

public interface ICompanyAPIService
{
    [Post("/api/Companies/v1")]
    Task CreateAsync([Body] CompanyCreateRequest request);
    
    [Get("/api/Companies/v1")]
    Task<PagedModel<Company>> GetCompaniesAsync([Query] PaginationFilter PaginationFilter);
    
    [Get("/api/Companies/v1/{id}")]
    Task<CompanyDetailResponse> GetCompanyAsync(Guid id);
    
    [Put("/api/Companies/v1")]
    Task<CompanyDetailResponse> UpdateCompany(UpdateCommpanyRequest request);

    [Delete("/api/Companies/v1/{id}")]
    Task Delete(Guid id);
}
