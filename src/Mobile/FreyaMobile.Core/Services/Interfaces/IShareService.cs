namespace FreyaMobile.Core.Services.Interfaces;

public interface IShareService
{
    Task<bool> DowloadOnGallery(Uri uri);

    Task ShareImage(Uri uri);

    Task ShareVideo(Uri uri);

    Task ShareSound(Uri uri);
}
