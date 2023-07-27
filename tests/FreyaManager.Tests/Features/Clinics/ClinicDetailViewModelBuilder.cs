using AppCenterServices;
using Freya.Desktop.Core.Services.Interrfaces;
using Freya.Desktop.Dialogs.Interfaces;
using Freya.Localization.Abstractions;
using FreyaManager.Features.Clinics;
using Freya.Desktop.Core.Features.Clinics;
using FreyaManager.Features.Clinics.Models;

namespace FreyaManager.Tests.Features.Clinics;

internal class ClinicDetailViewModelBuilder
{
    internal readonly Mock<IClinicService> clinicService;
    private readonly Mock<ClinicDetailEmployeesViewModel> clinicDetailEmployeesViewModel;
    private readonly Mock<ClinicDetailClientsViewModel> clinicDetailClientsViewModel;
    private readonly Mock<IDialogService> dialogService;
    private readonly Mock<IServiceProvider> serviceProvider;
    internal readonly Mock<ISessionService> sessionService;
    private readonly Mock<INavigationService> navigationService;
    private readonly Mock<ITranslatorService> translatorService;
    private readonly Mock<AppCenterAnalitics> appCenterAnalitics;

    public ClinicDetailViewModelBuilder()
    {
        clinicDetailEmployeesViewModel = new Mock<ClinicDetailEmployeesViewModel>(null, null, null);
        clinicDetailClientsViewModel = new Mock<ClinicDetailClientsViewModel>(null, null, null, null);
        clinicService = new Mock<IClinicService>();
        dialogService = new Mock<IDialogService>();
        sessionService = new Mock<ISessionService>();
        serviceProvider = new Mock<IServiceProvider>();
        navigationService = new Mock<INavigationService>();
        translatorService = new Mock<ITranslatorService>();
        appCenterAnalitics = new Mock<AppCenterAnalitics>();
    }

    internal ClinicDetailViewModel Build()
    {
        return new ClinicDetailViewModel(
            clinicService.Object,
            navigationService.Object,
            sessionService.Object,
            clinicDetailEmployeesViewModel.Object,
            clinicDetailClientsViewModel.Object,
            serviceProvider.Object,
            translatorService.Object,
            dialogService.Object,
            appCenterAnalitics.Object);
    }

    internal ClinicDetailViewModelBuilder WithClinic(ClinicDetailResponse clinic)
    {
        clinicService.Setup(x => x.GetClinic(It.Is<Guid>(x => x == clinic.Id))).ReturnsAsync(clinic);
        return this;
    }

    internal ClinicDetailViewModelBuilder SaveOnSession<T>(string SessionKey, object clientResponse)
    {
        sessionService.Setup(x => x.Get<T>(It.Is<string>(y => y == SessionKey))).Returns((T)clientResponse);

        return this;
    }
}
