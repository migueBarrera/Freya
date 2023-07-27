namespace Freya.Features.Employees;

public class CheckEmployeeViewModel : CoreViewModel
{
    private string email;
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private readonly ISessionService sessionService;
    private readonly ICheckEmployeeService checkEmployeeService;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private Guid clinicId;

    public CheckEmployeeViewModel(
        INavigationService navigationService,
        ICheckEmployeeService checkEmployeeService,
        IDialogService dialogService,
        ISessionService sessionService,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        this.navigationService = navigationService;
        this.sessionService = sessionService;
        this.translatorService = translatorService;
        this.checkEmployeeService = checkEmployeeService;
        this.dialogService = dialogService;
        this.appCenterAnalitics = appCenterAnalitics;

        email = string.Empty;

        CheckEmployeeCommand = new AsyncCommand(CheckEmployeeCommandExecute, IsValidEmail);
        BackCommand = new AsyncCommand(BackCommandExecute);

        this.ShowBackButton = true;
        Title = translatorService.Translate("page_title_add_employee");
        appCenterAnalitics.PageView(nameof(CheckEmployeeViewModel));
    }

    public string Email
    {
        get => email;
        set
        {
            SetAndRaisePropertyChanged(ref email, value);
            CheckEmployeeCommand.RaiseCanExecuteChanged();
        }
    }

    public AsyncCommand BackCommand { get; set; }

    public AsyncCommand CheckEmployeeCommand { get; set; }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        clinicId = sessionService.Get<Guid>(SESSION.KEY_CLINIC_ID_SELECTED);
        return base.OnAppearingAsync();
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    private async Task CheckEmployeeCommandExecute()
    {
        appCenterAnalitics.Clicked("Check employee");
        IsBusy = true;
        var result = await checkEmployeeService.CheckEmployeeState(Email, clinicId);
        IsBusy = false;

        if (result.IsSuccess)
        {
            await EvaluateResponse(result.Value);
        }
    }

    private bool IsValidEmail(object arg)
    {
        return EmailValidatorService.IsValidEmail(email);
    }

    private async Task EvaluateResponse(NewEmployeeState newEmployeeState)
    {
        appCenterAnalitics.AddEmployee(newEmployeeState.ToString());
        switch (newEmployeeState)
        {
            case NewEmployeeState.EMPLOYEE_NOT_REGISTERED:
                await dialogService.ShowMessageYesNo(
                    translatorService.Translate("dialog_check_employee_want_register_question_title"),
                    translatorService.Translate("dialog_check_employee_want_register_question_body"), 
                    new AsyncCommand(async () =>
                    {
                        sessionService.Save(SESSION.KEY_NEW_EMPLOYEE_EMAIL_PRESELECTED, Email);
                        await navigationService.NavigateTo(typeof(CreateNewEmployeePage));
                    }));
                break;
            case NewEmployeeState.EMPLOYEE_REGISTERED_FOR_OTHER_COMPANY:
                await dialogService.ShowMessage(
                    translatorService.Translate("dialog_check_employee_asigned_to_other_company_title"),
                    translatorService.Translate("dialog_check_employee_asigned_to_other_company_body"));
                break;
            case NewEmployeeState.CAN_NOT_REGISTER_COMPANY_EMPLOYEE_TO_INDIVIDUAL_CLINIC:
                await dialogService.ShowMessage(
                    translatorService.Translate("dialog_register_employee_can_not_register_employee_manager_to_individual_clinic_title"),
                    translatorService.Translate("dialog_register_employee_can_not_register_employee_manager_to_individual_clinic_body"));
                break;
            case NewEmployeeState.EMPLOYEE_REGISTERED_AND_ASIGNED:
                await dialogService.ShowMessage(
                    translatorService.Translate("dialog_check_employee_asigned_before_title"),
                    translatorService.Translate("dialog_check_employee_asigned_before_body"));
                break;
            case NewEmployeeState.EMPLOYEE_REGISTERED_BUT_NOT_ASIGNED:
                await dialogService.ShowMessageYesNo(
                    translatorService.Translate("dialog_check_employee_asigned_question_title"),
                    translatorService.Translate("dialog_check_employee_asigned_question_body"),
                    new AsyncCommand(async () =>
                    {
                        IsBusy = true;
                        var result = await checkEmployeeService.AssignEmployeeToSelectedClinic(Email, clinicId);
                        IsBusy = false;
                        if (result.IsSuccess)
                        {
                            await dialogService.ShowMessage(
                                translatorService.Translate("dialog_check_employee_asigned_success_title"),
                                translatorService.Translate("dialog_check_employee_asigned_success_body"));
                            await navigationService.BackAsync();
                        }
                    }));
                break;
        }
    }
}
