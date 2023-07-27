using System.Windows.Input;

namespace Freya.Features.Clinics.Models;

public class ClinicItemViewModel : ObservableObject,
    INavigationAwareViewModel
{
    private readonly INavigationService navigationService;
    private readonly ISessionService sessionService;
    public ClinicResponse? Clinic { get; private set; }

    public ClinicItemViewModel(INavigationService navigationService, ISessionService sessionService)
    {
        ViewDetailsCommand = new AsyncCommand(ViewDetailsCommandExecute);
        this.navigationService = navigationService;
        this.sessionService = sessionService;
    }

    private async Task ViewDetailsCommandExecute()
    {
        sessionService.Save(SESSION.KEY_CLINIC_ID_SELECTED, Clinic?.Id);
        sessionService.Save(SESSION.KEY_CLINIC_NAME_SELECTED, Clinic?.Name);
        await navigationService.NavigateTo(typeof(ClinicDetailPage), false);
    }

    public Task OnAppearingAsync(object? parameter = null)
    {
        Clinic = (parameter as ClinicResponse) ?? new ClinicResponse();
        return Task.CompletedTask;
    }

    public Task OnDisappearingAsync()
    {
        return Task.CompletedTask;
    }

    public ICommand ViewDetailsCommand { get; set; }
}
