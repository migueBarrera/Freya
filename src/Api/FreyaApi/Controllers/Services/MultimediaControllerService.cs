namespace FreyaApi.Controllers.Services;

public class MultimediaControllerService
{
    private readonly MultimediaService multimediaService;
    private readonly IVideoRepository videoRepository;
    private readonly ISoundRepository soundRepository;
    private readonly IImageRepository imageRepository;

    public MultimediaControllerService(
        MultimediaService multimediaService,
        IVideoRepository videoRepository,
        ISoundRepository soundRepository,
        IImageRepository imageRepository)
    {
        this.multimediaService = multimediaService;
        this.videoRepository = videoRepository;
        this.soundRepository = soundRepository;
        this.imageRepository = imageRepository;
    }

    public async Task<IActionResult> Delete(ControllerBase context, Guid id)
    {
        var isDeleted = await multimediaService.DeleteGenericAndReduceSize(id);
        if (isDeleted)
        {
            return context.Ok();
        }

        return context.NotFound();
    }

    public async Task<ActionResult<Guid>> AssingMultimediaToClient(ControllerBase context, CreateMultimediaRequest request)
    {
        var responseGuid = Guid.Empty;
        var baseMultimedia = ParseFromRequestToBaseMultimedia(request);
        if (baseMultimedia == null)
        {
            return context.BadRequest(request);
        }

        responseGuid = await InsertMultimediaDependOfType(responseGuid, baseMultimedia);

        return context.Ok(responseGuid);
    }

    private async Task<Guid> InsertMultimediaDependOfType(Guid responseGuid, Infrastructure.Models.BaseMultimedia? baseMultimedia)
    {
        switch (baseMultimedia)
        {
            case VideoTable video:
                {
                    var newVideoId = await videoRepository.AddVideo(video);

                    responseGuid = newVideoId;
                    break;
                }

            case SoundTable sound:
                {
                    Guid newSoundId = await soundRepository.AddSound(sound);

                    responseGuid = newSoundId;
                    break;
                }

            case UltrasoundTable ultrasound:
                {
                    Guid newUltrasoundId = await imageRepository.AddUltrasounds(ultrasound);

                    responseGuid = newUltrasoundId;
                    break;
                }
        }

        return responseGuid;
    }

    private static Infrastructure.Models.BaseMultimedia? ParseFromRequestToBaseMultimedia(CreateMultimediaRequest request)
    {
        switch (request.Type)
        {
            case MultimediaType.ULTRASOUND:
                return new UltrasoundTable()
                {
                    ClientId = request.ClientId,
                    ClinicId = request.ClinicId,
                    Uri = request.Uri,
                    Size = request.Size,
                };
            case MultimediaType.VIDEO:
                return new VideoTable()
                {
                    ClientId = request.ClientId,
                    ClinicId = request.ClinicId,
                    Uri = request.Uri,
                    Size = request.Size,
                    ThumnailUri = request.ThumnailUri,
                };
            case MultimediaType.SOUND:
                return new SoundTable()
                {
                    ClientId = request.ClientId,
                    ClinicId = request.ClinicId,
                    Uri = request.Uri,
                    Size = request.Size,
                };
            default:
                return null;
        }
    }
}
