using Models.Core.Clinics;
using System.Windows.Input;

namespace FreyaManager.Features.Companies.Models;

public class ClinicModel : ObservableObject
{
    private readonly INavigationService navigationService;
    private readonly ISessionService sessionService;

    private ClinicResponse _clinic;
    public ClinicModel(ClinicResponse item, INavigationService navigationService, ISessionService sessionService)
    {
        _clinic = item;
        Name = item.Name;

        GoToDetailsCommand = new AsyncCommand(GoToDetailsCommandExecute);
        this.navigationService = navigationService;
        this.sessionService = sessionService;
    }

    private async Task GoToDetailsCommandExecute()
    {
        sessionService.Save(SESSION.KEY_CLINIC_SELECTED, _clinic);
        await navigationService.NavigateTo(typeof(ClinicDetailPage));
    }

    public string Name { get; set; } = string.Empty;

    public ICommand GoToDetailsCommand { get; set; }
}
