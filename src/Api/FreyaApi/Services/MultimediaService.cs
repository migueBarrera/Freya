namespace FreyaApi.Services;

public class MultimediaService
{
    private readonly WasabiUploadService azureUploaderService;
    private readonly IVideoRepository videoRepository;
    private readonly ISoundRepository soundRepository;
    private readonly IImageRepository imageRepository;

    public MultimediaService(
        WasabiUploadService azureUploaderService,
        IVideoRepository videoRepository,
        ISoundRepository soundRepository,
        IImageRepository imageRepository)
    {
        this.azureUploaderService = azureUploaderService;
        this.videoRepository = videoRepository;
        this.soundRepository = soundRepository;
        this.imageRepository = imageRepository;
    }

    public async Task<bool> DeleteGenericAndReduceSize(Guid id)
    {
        var isAVideo = videoRepository.VideoExist(id);
        if (isAVideo)
        {
            await InternalDeleteVideo(id);
            return true;
        }

        var isAImage = imageRepository.ImageExist(id);
        if (isAImage)
        {
            await InternalDeleteImage(id);
            return true;
        }

        var isASound = soundRepository.SoundExist(id);
        if (isASound)
        {
            await InternalDeleteSounds(id);
            return true;
        }

        return false;
    }

    public async Task DeleteAllMultimediaOfAClientOnAClinic(Guid clientId, Guid clinicId)
    {
        IEnumerable<VideoTable> videos = await videoRepository.GetVideos(clientId, clinicId);
        if (videos.Any())
        {
            await InternalDeleteRangeVideo(clientId, videos);
        }

        IEnumerable<UltrasoundTable> images = await imageRepository.GetImages(clientId, clinicId);
        if (images.Any())
        {
            await InternalDeleteRangeImages(clientId, images);
        }

        IEnumerable<SoundTable> sounds = await soundRepository.GetSounds(clientId, clinicId);
        if (sounds.Any())
        {
            await InternalDeleteRangeSounds(clientId, sounds);
        }
    }

    private async Task<bool> InternalDeleteRangeVideo(Guid clientId, IEnumerable<VideoTable> videos)
    {
        var deleted = await videoRepository.RemoveVideos(videos.Select(x => x.Id));

        await azureUploaderService.RemoveRange(
            BucketHelper.ToClientBucket(clientId),
            videos.Select(x => x.Uri.Segments.Last().ToString()).ToArray());

        var thumnails = videos.Where(x => x.ThumnailUri != null).Select(x => x.ThumnailUri).ToArray();
        if (thumnails?.Any() == true)
        {
            string[] thumnailUris = thumnails
                .Where(uri => uri != null)
                .Select(uri => uri!.Segments.Last().ToString()).ToArray();

            await azureUploaderService.RemoveRange(
                BucketHelper.ToClientBucket(clientId),
                thumnailUris);
        }

        return true;
    }

    private async Task<bool> InternalDeleteRangeImages(Guid clientId, IEnumerable<UltrasoundTable> images)
    {
        var deleted = await imageRepository.RemoveUltraSounds(images.Select(x => x.Id));
        await azureUploaderService.RemoveRange(
            BucketHelper.ToClientBucket(clientId),
            images.Select(x => x.Uri.Segments.Last().ToString()).ToArray());
        return true;
    }

    private async Task<bool> InternalDeleteRangeSounds(Guid clientId, IEnumerable<SoundTable> sounds)
    {
        var totalSize = sounds.Sum(x => x.Size);
        var deleted = await soundRepository.RemoveSounds(sounds.Select(x => x.Id));
        await azureUploaderService.RemoveRange(
            BucketHelper.ToClientBucket(clientId),
            sounds.Select(x => x.Uri.Segments.Last().ToString()).ToArray());
        return true;
    }

    private async Task<bool> InternalDeleteVideo(Guid videoId)
    {
        VideoTable? item = await videoRepository.GetVideo(videoId);

        if (item != null)
        {
            var deleted = await videoRepository.RemoveVideo(videoId);
            await azureUploaderService.DeleteFile(BucketHelper.ToClientBucket(item.ClientId), item.Uri.Segments.Last());
            if (item.ThumnailUri != null)
            {
                await azureUploaderService.DeleteFile(BucketHelper.ToClientBucket(item.ClientId), item.ThumnailUri.Segments.Last());
            }
            return true;
        }

        return false;
    }

    private async Task<bool> InternalDeleteImage(Guid imageId)
    {
        UltrasoundTable? item = await imageRepository.GetUltrasound(imageId);
        if (item != null)
        {
            var deleted = await imageRepository.RemoveUltraSound(imageId);
            await azureUploaderService.DeleteFile(BucketHelper.ToClientBucket(item.ClientId), item.Uri.Segments.Last());
            return true;
        }

        return false;
    }

    private async Task<bool> InternalDeleteSounds(Guid soundId)
    {
        SoundTable? item = await soundRepository.GetSound(soundId);

        if (item != null)
        {
            var deleted = await soundRepository.RemoveSound(soundId);
            await azureUploaderService.DeleteFile(BucketHelper.ToClientBucket(item.ClientId), item.Uri.Segments.Last());
            return true;
        }

        return false;
    }
}
