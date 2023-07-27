using Freya.Desktop.Core.Helpers;

namespace Freya.Features.Employees;

public class CreateNewEmployeeViewModel : CoreViewModel
{
    private readonly ICreateNewEmployeeService createNewEmployeeService;
    private readonly IDialogService dialogService;
    private readonly ICurrentEmployeeService currentEmployeeService;
    private readonly ISessionService sessionService;
    private readonly ITranslatorService translatorService;
    private readonly IEmployeeRolService employeeRolService;
    private readonly INavigationService navigationService;
    private readonly AppCenterAnalitics appCenterAnalitics;
   
    private bool hasEmailPreseleted;
    private CreateEmployeeModel createEmployeeModel;

    public CreateNewEmployeeViewModel(
        ICurrentEmployeeService currentEmployeeService,
        IDialogService dialogService,
        ISessionService sessionService,
        ICreateNewEmployeeService createNewEmployeeService,
        INavigationService navigationService,
        ITranslatorService translatorService,
        IEmployeeRolService employeeRolService,
        AppCenterAnalitics appCenterAnalitics)
    {
        Title = translatorService.Translate("page_title_new_employee");
        ShowBackButton = true;
        createEmployeeModel = new CreateEmployeeModel();

        CreateEmployeeCommand = new AsyncCommand(CreateEmployeeCommandExecute);
        BackCommand = new AsyncCommand(BackCommandExecute);
        this.currentEmployeeService = currentEmployeeService;
        this.dialogService = dialogService;
        this.sessionService = sessionService;
        this.createNewEmployeeService = createNewEmployeeService;
        this.navigationService = navigationService;
        this.translatorService = translatorService;
        this.employeeRolService = employeeRolService;
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(CreateNewEmployeeViewModel));
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    public IAsyncCommand BackCommand { get; set; }

    public IAsyncCommand CreateEmployeeCommand { get; set; }

    public bool HasEmailPreseleted { get => hasEmailPreseleted; set => SetAndRaisePropertyChanged(ref hasEmailPreseleted, value); }
    public CreateEmployeeModel CreateEmployeeModel { get => createEmployeeModel; set => SetAndRaisePropertyChanged(ref createEmployeeModel, value); }
    

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();

        createEmployeeModel.RolList = GetRolListCapsule();
        createEmployeeModel.RolSelected = EmployeeRol.CLINIC_OFFICER;

        var emialPreselected = sessionService.Get<string>(SESSION.KEY_NEW_EMPLOYEE_EMAIL_PRESELECTED);
        if (!string.IsNullOrWhiteSpace(emialPreselected))
        {
            createEmployeeModel.EmployeeEmail = emialPreselected;
            HasEmailPreseleted = true;
        }
    }

    private IEnumerable<CapsuleRol> GetRolListCapsule()
    {
        var isClinicManagerOrHigher = employeeRolService.IsClinicManagerOrHigher(currentEmployeeService.Employee?.Rol ?? EmployeeRol.CLINIC_OFFICER);
        var listRoles = isClinicManagerOrHigher
            ? new List<EmployeeRol>() { EmployeeRol.CLINIC_MANAGER, EmployeeRol.CLINIC_OFFICER }
            : new List<EmployeeRol>() { EmployeeRol.CLINIC_OFFICER };
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



    private async Task CreateEmployeeCommandExecute()
    {
        if (string.IsNullOrWhiteSpace(createEmployeeModel.EmployeeEmail) ||
            string.IsNullOrWhiteSpace(createEmployeeModel.EmployeeName))
        {
            await dialogService.ShowMessage(
                translatorService.Translate("error"),
                translatorService.Translate("check_fields"));
            return;
        }

        appCenterAnalitics.Clicked("Create employee");

        var clinicId = sessionService.Get<Guid>(SESSION.KEY_CLINIC_ID_SELECTED);
        var currentEmployee = currentEmployeeService.Employee;
        IsBusy = true;
        var result = await createNewEmployeeService.CreateEmployee(
            new Employee()
            {
                Email = createEmployeeModel.EmployeeEmail,
                LastName = createEmployeeModel.EmployeeLastName,
                Name = createEmployeeModel.EmployeeName,
                Rol = createEmployeeModel.RolSelected,
                CompanyId = currentEmployee!.CompanyId,
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
