using Models.Core.Companies;
using System.Windows.Input;

namespace FreyaManager.Features.Companies.Models;

public class CompanyModel : ObservableObject
{
    private INavigationService navigationService;
    private ISessionService sessionService;

    private Company _company;
    public CompanyModel(Company item, INavigationService navigationService, ISessionService sessionService)
    {
        _company = item;
        Name = item.Name;

        GoToDetailsCommand = new AsyncCommand(GoToDetailsCommandExecute);
        this.navigationService = navigationService;
        this.sessionService = sessionService;
    }

    private async Task GoToDetailsCommandExecute()
    {
        sessionService.Save(SESSION.KEY_COMPANY_SELECTED, _company);
        await navigationService.NavigateTo(typeof(DetailCompanyPage));
    }

    public string Name { get; set; } = string.Empty;

    public ICommand GoToDetailsCommand { get; set; }
}
