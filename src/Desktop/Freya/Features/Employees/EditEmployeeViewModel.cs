namespace Freya.Features.Employees;

public class EditEmployeeViewModel : CoreViewModel
{
    private EmployeeResponse? internalEmployee;
    private EditEmployeeModel editEmployeeModel;
    private readonly INavigationService navigationService;
    private readonly ISessionService sessionService;
    private readonly IEditEmployeeService editEmployeeService;
    private readonly AppCenterAnalitics appCenterAnalitics;

    public EditEmployeeViewModel(
        INavigationService navigationService,
        ISessionService sessionService,
        IEditEmployeeService editEmployeeService,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        ShowBackButton = true;
        Title = translatorService.Translate("page_title_edit_employee");
        this.navigationService = navigationService;
        editEmployeeModel = new EditEmployeeModel();
        SaveChangesEditEmployeeCommand = new AsyncCommand(SaveChangesEmployeeCommandExecute);
        BackCommand = new AsyncCommand(BackCommandExecute);
        this.sessionService = sessionService;
        this.editEmployeeService = editEmployeeService;
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(EditEmployeeViewModel));
    }

    public IAsyncCommand BackCommand { get; set; }

    public IAsyncCommand SaveChangesEditEmployeeCommand { get; set; }

    public EditEmployeeModel EditEmployeeModel { get => editEmployeeModel; set => SetAndRaisePropertyChanged(ref editEmployeeModel, value); }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        internalEmployee = sessionService.Get<EmployeeResponse>(SESSION.KEY_EMPLOYEE_ITEM_SELECTED);
        EditEmployeeModel.EmployeeEmail = internalEmployee.Email;
        EditEmployeeModel.EmployeeName = internalEmployee.Name;
        EditEmployeeModel.EmployeeLastName = internalEmployee.LastName;
        return base.OnAppearingAsync(parameter);
    }

    private async Task SaveChangesEmployeeCommandExecute()
    {
        appCenterAnalitics.Clicked("Save employee data");
        IsBusy = true;
        var result = await editEmployeeService.SaveEmployeeData(new EmployeeUpdateRequest()
        {
            Name = editEmployeeModel.EmployeeName,
            LastName = editEmployeeModel.EmployeeLastName,
            Id = internalEmployee!.Id,
        });

        IsBusy = false;

        if (result)
        {
            await navigationService.BackAsync();
        }
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }
}
