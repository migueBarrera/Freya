using Models.Core.Employees;
using OperationResult;

namespace FreyaManager.Features.Employees;

public class CheckEmployeeViewModel : CoreViewModel
{
    private string email;
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private readonly ISessionService sessionService;
    private readonly CheckEmployeeService checkEmployeeService;
    private readonly ITranslatorService translatorService;
    private Guid clinicId;
    private Guid companyId;
    private bool wantAddToAClinic;

    public CheckEmployeeViewModel(
        INavigationService navigationService,
        CheckEmployeeService checkEmployeeService,
        IDialogService dialogService,
        ISessionService sessionService,
        ITranslatorService translatorService)
    {
        this.navigationService = navigationService;
        this.sessionService = sessionService;
        this.translatorService = translatorService;
        this.checkEmployeeService = checkEmployeeService;
        this.dialogService = dialogService;

        email = string.Empty;

        CheckEmployeeCommand = new AsyncCommand(CheckEmployeeCommandExecute, IsValidEmail);
        BackCommand = new AsyncCommand(BackCommandExecute);

        this.ShowBackButton = true;
        Title = translatorService.Translate("page_title_add_employee");
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
        wantAddToAClinic = sessionService.Get<bool>(SESSION.KEY_NEW_EMPLOYEE_FOR_CLINIC);
        if (wantAddToAClinic)
        {
            clinicId = sessionService.Get<Guid>(SESSION.KEY_NEW_EMPLOYEE_FOR_CLINIC_ID);
        }
        else
        {
            companyId = sessionService.Get<Guid>(SESSION.KEY_COMPANY_ID_SELECTED);
        }
        return base.OnAppearingAsync();
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    private async Task CheckEmployeeCommandExecute()
    {
        IsBusy = true;
        Result<NewEmployeeState> result;
        if (wantAddToAClinic)
        {
            result = await checkEmployeeService.CheckEmployeeState(Email, clinicId);
        }
        else
        {
            result = await checkEmployeeService.CheckEmployeeStateForACompany(Email, companyId);
        }
        IsBusy = false;

        if (result.IsSuccess)
        {
            if (wantAddToAClinic)
            {
                await EvaluateResponseForAddToClinic(result.Value);
            }
            else
            {
                await EvaluateResponseForAddToCompany(result.Value);
            }
        }
    }

    private bool IsValidEmail(object arg)
    {
        return EmailValidatorService.IsValidEmail(email);
    }

    private async Task EvaluateResponseForAddToCompany(NewEmployeeState newEmployeeState)
    {
        switch (newEmployeeState)
        {
            case NewEmployeeState.EMPLOYEE_NOT_REGISTERED:
                await dialogService.ShowMessageYesNo(
                    translatorService.Translate("dialog_check_employee_want_register_question_title"),
                    translatorService.Translate("dialog_check_employee_want_register_question_body"),
                    new AsyncCommand(async () =>
                    {
                        sessionService.Save(SESSION.KEY_NEW_EMPLOYEE_EMAIL_PRESELECTED, Email);
                        await navigationService.NavigateTo(typeof(CreateEmployeePage));
                    }));
                break;
            case NewEmployeeState.EMPLOYEE_REGISTERED_AND_ASIGNED:
                await dialogService.ShowMessage(
                    translatorService.Translate("dialog_check_employee_asigned_before_for_a_company_title"),
                    translatorService.Translate("dialog_check_employee_asigned_before_for_a_company_body"));
                break; 
            case NewEmployeeState.EMPLOYEE_REGISTERED_FOR_OTHER_COMPANY:
                await dialogService.ShowMessage(
                    translatorService.Translate("dialog_check_employee_asigned_to_other_company_title"),
                    translatorService.Translate("dialog_check_employee_asigned_to_other_company_body"));
                break;
        }
    }
    private async Task EvaluateResponseForAddToClinic(NewEmployeeState newEmployeeState)
    {
        switch (newEmployeeState)
        {
            case NewEmployeeState.EMPLOYEE_NOT_REGISTERED:
                await dialogService.ShowMessageYesNo(
                    translatorService.Translate("dialog_check_employee_want_register_question_title"),
                    translatorService.Translate("dialog_check_employee_want_register_question_body"),
                    new AsyncCommand(async () =>
                    {
                        sessionService.Save(SESSION.KEY_NEW_EMPLOYEE_EMAIL_PRESELECTED, Email);
                        await navigationService.NavigateTo(typeof(CreateEmployeePage));
                    }));
                break;
            case NewEmployeeState.EMPLOYEE_REGISTERED_AND_ASIGNED:
                await dialogService.ShowMessage(
                    translatorService.Translate("dialog_check_employee_asigned_before_title"),
                    translatorService.Translate("dialog_check_employee_asigned_before_body"));
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
