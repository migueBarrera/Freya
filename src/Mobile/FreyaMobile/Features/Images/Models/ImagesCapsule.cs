namespace FreyaMobile.Features.Images.Models;

public class ImagesCapsule
{
    public IEnumerable<ImageModel> Images { get; set; } = Enumerable.Empty<ImageModel>();

    public int Position { get; set; }
}
