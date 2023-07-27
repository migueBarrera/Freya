using Models.Core.Common;

namespace Freya.Features.Employees.Models;

public class EmployeeItemViewModel : ObservableObject, INavigationAwareViewModel, IBusyViewModel
{
    private bool isBusy;
    private bool canDeleteEmployee;
    private bool canEditEmployee;
    private readonly IDialogService dialogService;
    private readonly IEmployeeService employeeService;
    private readonly ICurrentClinicService currentClinicService;
    private readonly INavigationService navigationService;
    private readonly ISessionService sessionService;
    private readonly ITranslatorService translatorService;
    private readonly ICurrentEmployeeService currentEmployeeService;
    private readonly IEmployeeRolService employeeRolService;
    public EmployeeResponse? Employee { get; private set; }
    public ClinicDetailEmployeesViewModel? ClinicDetailViewModel { set; get; }

    public EmployeeItemViewModel(
        IDialogService dialogService,
        IEmployeeService employeeService,
        ICurrentClinicService currentClinicService,
        INavigationService navigationService,
        ISessionService sessionService,
        ITranslatorService translatorService,
        ICurrentEmployeeService currentEmployeeService,
        IEmployeeRolService employeeRolService)
    {
        DeleteEmployeeCommand = new AsyncCommand(DeleteEmployeeCommandExecute);
        EditEmployeeCommand = new AsyncCommand(EditEmployeeCommandExecute);
        this.dialogService = dialogService;
        this.employeeService = employeeService;
        this.currentClinicService = currentClinicService;
        this.navigationService = navigationService;
        this.sessionService = sessionService;
        this.translatorService = translatorService;
        this.currentEmployeeService = currentEmployeeService;
        this.employeeRolService = employeeRolService;
    }

    private async Task DeleteEmployeeCommandExecute()
    {
        var clinic = currentClinicService.CurrentClinic;
        await dialogService.ShowMessageYesNo(
                translatorService.Translate("remove_employee_title"),
                translatorService.Translate("remove_employee_message"),
                new AsyncCommand(async () =>
                {
                    IsBusy = true;
                    var deleted = await employeeService.DeleteEmployeeFromClinic(Employee!.Id, clinic!.Id);
                    IsBusy = false;
                    if (deleted)
                    {
                        ClinicDetailViewModel!.Employees.Items.Remove(this);
                        var x = new PagedModel<EmployeeItemViewModel>(
                            ClinicDetailViewModel.Employees.Items,
                            ClinicDetailViewModel.Employees.TotalCount--,
                            ClinicDetailViewModel.Employees.CurrentPage,
                            ClinicDetailViewModel.Employees.PageSize,
                            ClinicDetailViewModel.Employees.SearchArgument);
                        
                        ClinicDetailViewModel.Employees = x;
                    }
                }));
    }

    private async Task EditEmployeeCommandExecute()
    {
        sessionService.Save(SESSION.KEY_EMPLOYEE_ITEM_SELECTED, Employee);
        await navigationService.NavigateTo(typeof(EditEmployeePage));
    }

    public IAsyncCommand DeleteEmployeeCommand { get; set; }

    public IAsyncCommand EditEmployeeCommand { get; set; }

    public bool IsBusy
    {
        get => isBusy;
        set => SetAndRaisePropertyChanged(ref isBusy, value);
    }

    public bool CanEditEmployee
    {
        get => canEditEmployee;
        set => SetAndRaisePropertyChanged(ref canEditEmployee, value);
    }
    
    public bool CanDeleteEmployee
    {
        get => canDeleteEmployee;
        set => SetAndRaisePropertyChanged(ref canDeleteEmployee, value);
    }

    public Task OnAppearingAsync(object? parameter = null)
    {
        Employee = parameter as EmployeeResponse ?? new EmployeeResponse();
        var isUserLogged = Employee.Id == currentEmployeeService.Employee?.Id;
        var IsClinicManagerOrHigher = employeeRolService.IsClinicManagerOrHigher(currentEmployeeService.Employee?.Rol ?? EmployeeRol.CLINIC_OFFICER);

        CanEditEmployee = IsClinicManagerOrHigher && !isUserLogged;
        CanDeleteEmployee = IsClinicManagerOrHigher && !isUserLogged;

        return Task.CompletedTask;
    }

    public Task OnDisappearingAsync()
    {
        return Task.CompletedTask;
    }
}
