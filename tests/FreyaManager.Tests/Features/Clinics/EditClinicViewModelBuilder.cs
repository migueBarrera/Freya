using AppCenterServices;
using Freya.Desktop.Core.Features.Clinics;
using Freya.Desktop.Core.Services.Interrfaces;
using Freya.Desktop.Dialogs.Interfaces;
using Freya.Localization.Abstractions;
using FreyaManager.Features.Clinics;
using Moq;

namespace FreyaManager.Tests.Features.Clinics;

internal class EditClinicViewModelBuilder
{
    private readonly Mock<IDialogService> dialogService;
    public readonly Mock<ISessionService> sessionService;
    private readonly Mock<IEditClinicService> editClinicService;
    private readonly Mock<INavigationService> navigationService;
    private readonly Mock<ITranslatorService> translatorService;
    private readonly Mock<AppCenterAnalitics> appCenterAnalitics;

    public EditClinicViewModelBuilder()
    {
        dialogService = new Mock<IDialogService>();
        sessionService = new Mock<ISessionService>();

        editClinicService = new Mock<IEditClinicService>();
        navigationService = new Mock<INavigationService>();
        translatorService = new Mock<ITranslatorService>();
        appCenterAnalitics = new Mock<AppCenterAnalitics>();
    }

    internal EditClinicViewModel Build()
    {
        return new EditClinicViewModel(
            sessionService.Object,
            navigationService.Object,
            editClinicService.Object,
            dialogService.Object,
            translatorService.Object,
            appCenterAnalitics.Object);
    }

    internal EditClinicViewModelBuilder SaveOnSession<T>(string SessionKey, object clientResponse)
    {
        sessionService.Setup(x => x.Get<T>(It.Is<string>(y => y == SessionKey))).Returns((T)clientResponse);

        return this;
    }
}
