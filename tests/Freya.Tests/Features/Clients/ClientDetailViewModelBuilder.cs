using ApiContract.Refit;
using Freya.Features.Clients;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Models.Core.Clients;
using Freya.Services;
using Freya.Services.UploadManager;
using Freya.Desktop.Core.Features.Clients.Services;
using Freya.Features.Clients.Services;

namespace Freya.Tests.Features.Clients;

internal class ClientDetailViewModelBuilder
{
    private readonly Mock<IDialogService> dialogService;
    public readonly Mock<ISessionService> sessionService;
    private readonly Mock<ICurrentClinicService> currentClinicService;
    private readonly Mock<INavigationService> navigationService;
    private readonly Mock<ITranslatorService> translatorService;
    private readonly Mock<AppCenterAnalitics> appCenterAnalitics;
    private readonly Mock<IClientDetailService> clientDetailService;
    private readonly Mock<ISessionManagerService> sessionManagerService;
    private readonly Mock<IMultimediaFilePickerService> multimediaFilePickerService;
    private ILogger<ClientDetailViewModel> logger = new NullLogger<ClientDetailViewModel>();

    public ClientDetailViewModelBuilder()
    {
        dialogService = new Mock<IDialogService>();
        sessionService = new Mock<ISessionService>();

        var refitService = new Mock<IRefitService>();
        currentClinicService = new Mock<ICurrentClinicService>();
        navigationService = new Mock<INavigationService>();
        translatorService = new Mock<ITranslatorService>();
        appCenterAnalitics = new Mock<AppCenterAnalitics>();
        clientDetailService = new Mock<IClientDetailService>();
        sessionManagerService = new Mock<ISessionManagerService>();
        multimediaFilePickerService = new Mock<IMultimediaFilePickerService>();
    }

    internal ClientDetailViewModel Build()
    {
        return new ClientDetailViewModel(
            sessionService.Object,
            logger,
            clientDetailService.Object,
            null!,
            dialogService.Object,
            navigationService.Object,
            sessionManagerService.Object,
            currentClinicService.Object,
            translatorService.Object,
            appCenterAnalitics.Object,
            multimediaFilePickerService.Object);
    }

    internal ClientDetailViewModelBuilder SaveOnSession<T>(string SessionKey, object clientResponse)
    {
        sessionService.Setup(x => x.Get<T>(It.Is<string>(y => y == SessionKey))).Returns((T)clientResponse);

        return this;
    }
    
    internal ClientDetailViewModelBuilder WithGetClientDetails()
    {
        clientDetailService.Setup(x => x.GetClientDetails(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new ClientDetailResponse());

        return this;
    }
}
