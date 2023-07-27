namespace FreyaMobile.Core.Services
{
    public class ShareService : IShareService
    {
        private readonly IDownloaderService downloaderService;
        private readonly INativeMediaService nativeMediaService;

        public ShareService(
            IDownloaderService downloaderService,
            INativeMediaService nativeMediaService)
        {
            this.downloaderService = downloaderService;
            this.nativeMediaService = nativeMediaService;
        }

        public async Task<bool> DowloadOnGallery(Uri uri)
        {
            var file = await downloaderService.DownloadFromUriAsync(uri);
            if (file != null)
            {
                return nativeMediaService.SaveImageFromByte(file, Path.GetFileName(uri.ToString()));
            }
            else
            {
                return false;
            }
        }

        public async Task ShareImage(Uri uri)
        {
            var file = await downloaderService.DownloadFromUriAsync(uri);
            if (file != null)
            {
                var extension = Path.GetExtension(uri.AbsolutePath);
                var fn = $"{Guid.NewGuid()}.{extension}";
                var filePath = Path.Combine(FileSystem.CacheDirectory, fn);
                File.WriteAllBytes(filePath, file);

                await Share.RequestAsync(new ShareFileRequest
                {
                    File = new ShareFile(filePath),
                    Title = "Share Web Link"
                });
            }
        }

        public async Task ShareSound(Uri uri)
        {
            var file = await downloaderService.DownloadFromUriAsync(uri);
            if (file != null)
            {
                var extension = Path.GetExtension(uri.AbsolutePath);
                var fn = $"{Guid.NewGuid()}.{extension}";
                var filePath = Path.Combine(FileSystem.CacheDirectory, fn);
                File.WriteAllBytes(filePath, file);

                await Share.RequestAsync(new ShareFileRequest
                {
                    File = new ShareFile(filePath),
                    Title = "Share Web Link"
                });
            }
        }

        public async Task ShareVideo(Uri uri)
        {
            var file = await downloaderService.DownloadFromUriAsync(uri);
            if (file != null)
            {
                var extension = Path.GetExtension(uri.AbsolutePath);
                var fn = $"{Guid.NewGuid()}.{extension}";
                var filePath = Path.Combine(FileSystem.CacheDirectory, fn);
                File.WriteAllBytes(filePath, file);

                await Share.RequestAsync(new ShareFileRequest
                {
                    File = new ShareFile(filePath),
                    Title = "Share Web Link"
                });
            }
        }
    }
}
