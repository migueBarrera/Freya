using Models.Core.Multimedia;

namespace ApiContract.Interfaces;

public interface IMultimediaAPIService
{
    [Post("/api/multimedia/v1")]
    Task<Guid> CreateMultimedia([Body] CreateMultimediaRequest request);
    
    [Delete("/api/multimedia/v1/{id}")]
    Task Delete(Guid id);
}
