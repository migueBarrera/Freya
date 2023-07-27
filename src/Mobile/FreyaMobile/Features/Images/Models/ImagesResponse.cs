using Models.Core.Multimedia;

namespace FreyaMobile.Features.Images.Models;

public class ImagesResponse
{
    public int Id { get; set; }

    public Uri? Url { get; set; }

    public int ClientId { get; set; }

    internal static ImageModel ConvertTo(BaseMultimedia dto) => new ImageModel() 
    {
        Image = dto.Uri,
    };
}
