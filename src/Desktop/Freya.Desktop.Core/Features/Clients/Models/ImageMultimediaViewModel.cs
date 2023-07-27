using Freya.Desktop.Core.Features.Clients.Services;
using Models.Core.Multimedia;

namespace Freya.Desktop.Core.Features.Clients.Models;

public class ImageMultimediaViewModel : BaseMultimediaViewModel
{
    private ImageMultimedia data = new();

    public ImageMultimediaViewModel(
        IDialogService dialogService,
        MultimediaService multimediaService,
        ILogger<BaseMultimediaViewModel> logger,
        IServiceProvider serviceProvider,
        ITranslatorService translatorService,
        AppCenterService appCenterService)
        : base(dialogService, multimediaService, logger, serviceProvider, translatorService, appCenterService)
    {
    }

    public ImageMultimedia Data { get => data; set => SetAndRaisePropertyChanged(ref data, value); }

    public override MultimediaType MultimediaType => MultimediaType.ULTRASOUND;

    internal override Guid GetMultimediaId()
    {
        return Data.Id;
    }

    internal override Uri GetMultimediaUri()
    {
        return Data.Uri;
    }
}
