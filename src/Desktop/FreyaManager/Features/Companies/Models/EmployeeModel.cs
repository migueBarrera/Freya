using Models.Core.Employees;
using System.Windows.Input;

namespace FreyaManager.Features.Companies.Models;

public class EmployeeModel : ObservableObject
{
    public string Email { get; set; } = string.Empty;
    public EmployeeRol Rol { get; set; } = EmployeeRol.CLINIC_OFFICER;

    private INavigationService navigationService;
    private ISessionService sessionService;

    private EmployeeResponse _company;
    public EmployeeModel(EmployeeResponse item, INavigationService navigationService, ISessionService sessionService)
    {
        _company = item;
        Name = item.Name;
        Rol = item.Rol;
        Email = item.Email;
        GoToDetailsCommand = new AsyncCommand(GoToDetailsCommandExecute);
        this.navigationService = navigationService;
        this.sessionService = sessionService;
    }


    private Task GoToDetailsCommandExecute()
    {
        sessionService.Save(SESSION.KEY_COMPANY_SELECTED, _company);
        //await navigationService.NavigateTo(typeof());
        return Task.CompletedTask;
    }

    public string Name { get; set; } = string.Empty;

    public ICommand GoToDetailsCommand { get; set; }
}
