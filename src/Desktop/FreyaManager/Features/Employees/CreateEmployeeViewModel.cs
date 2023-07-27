using Models.Core.Companies;
using Models.Core.Employees;
using Freya.Desktop.Core.Helpers;

namespace FreyaManager.Features.Employees;

public class CreateEmployeeViewModel : CoreViewModel
{
    private readonly EmployeeService _employeeService;
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private readonly ISessionService sessionService;
    private readonly ITranslatorService translatorService;
    private Guid clinicId = Guid.Empty;
    private IEnumerable<CapsuleRol> rolList = Enumerable.Empty<CapsuleRol>();
    private bool hasEmailPreseleted;

    public CreateEmployeeViewModel(
        EmployeeService employeeService,
        IDialogService dialogService,
        INavigationService navigationService,
        ISessionService sessionService,
        ITranslatorService translatorService)
    {
        ShowBackButton = true;
        Title = translatorService.Translate("page_title_new_employee");

        _employeeService = employeeService;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
        this.sessionService = sessionService;
        this.translatorService = translatorService;

        CreateEmployeeCommand = new AsyncCommand(CreateEmployeeCommandExecute);
        BackCommand = new AsyncCommand(BackCommandExecute);

        NewEmployee = new NewEmployeeValidatable();
    }

    public IAsyncCommand CreateEmployeeCommand { get; set; }

    public IAsyncCommand BackCommand { get; set; }

    public Company? Company { get; set; }

    public NewEmployeeValidatable NewEmployee { get; set; }

    public IEnumerable<CapsuleRol> RolList { get => rolList; set => SetAndRaisePropertyChanged(ref rolList, value); }

    public bool HasEmailPreseleted { get => hasEmailPreseleted; set => SetAndRaisePropertyChanged(ref hasEmailPreseleted, value); }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        var emialPreselected = sessionService.Get<string>(SESSION.KEY_NEW_EMPLOYEE_EMAIL_PRESELECTED);
        if (!string.IsNullOrWhiteSpace(emialPreselected))
        {
            NewEmployee.EmployeeEmail.Value = emialPreselected;
            HasEmailPreseleted = true;
        }

        Company = sessionService.Get<Company>(SESSION.KEY_COMPANY_SELECTED);
        RolList = GetRolListCapsule();
        NewEmployee.RolSelected.Value = RolList.First().Rol;

        return base.OnAppearingAsync();
    }

    private IEnumerable<CapsuleRol> GetRolListCapsule()
    {
        var isANewEmployeeForAClinic = sessionService.Get<bool>(SESSION.KEY_NEW_EMPLOYEE_FOR_CLINIC);

        if (isANewEmployeeForAClinic)
        {
            clinicId = sessionService.Get<Guid>(SESSION.KEY_NEW_EMPLOYEE_FOR_CLINIC_ID);
        }

        var listRoles = isANewEmployeeForAClinic
            ? new List<EmployeeRol>() { EmployeeRol.CLINIC_MANAGER, EmployeeRol.CLINIC_OFFICER }
            : new List<EmployeeRol>() { EmployeeRol.COMPANY_OWNER, EmployeeRol.COMPANY_MANAGER };
        var listCapule = new List<CapsuleRol>();
        foreach (var item in listRoles)
        {
            var capsule = new CapsuleRol()
            {
                Rol = item,
                Text = translatorService.GetTextByRol(item),
            };
            listCapule.Add(capsule);
        }

        return listCapule;
    }

    private async Task BackCommandExecute()
    {
        await navigationService.BackAsync();
    }

    private async Task CreateEmployeeCommandExecute()
    {
        if (NewEmployee.HasError(out var error))
        {
            await dialogService.ShowMessage(
                "Error",
                error);
            return;
        }

        IsBusy = true; 
        var result = await _employeeService.CreateEmployee(
            new EmployeeSignUpRequest()
            {
                Email = NewEmployee.EmployeeEmail.Value,
                LastName = NewEmployee.EmployeeLastName.Value,
                Name = NewEmployee.EmployeeName.Value,
                Rol = NewEmployee.RolSelected.Value,
                CompanyId = Company!.Id,
            },
            clinicId);
        IsBusy = false;

        if (result)
        {
            await dialogService.ShowMessage(
                translatorService.Translate("dialog_create_new_employee_success_title"),
                translatorService.Translate("dialog_create_new_employee_success_body"));
            await navigationService.BackAsync();
            await navigationService.BackAsync();
        }
    }
}
