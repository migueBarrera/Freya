using FreyaMobile.Features.Images.Models;

namespace FreyaMobile.Features.Images.Services;

public interface IImagesService
{
    Task<IEnumerable<ImageModel>> GetImagesAsync();
}
