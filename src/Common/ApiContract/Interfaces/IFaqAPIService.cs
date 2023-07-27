using Models.Core.FAQ;

namespace ApiContract.Interfaces;

public interface IFaqAPIService
{
    [Post("/api/faq/v1")]
    Task Create(FAQ request);
    
    [Put("/api/faq/v1")]
    Task Update(FAQ request);
    
    [Delete("/api/faq/v1/{id}")]
    Task Delete(Guid id);

    [Get("/api/faq/v1")]
    Task<IEnumerable<FAQ>> GetAll();
}
