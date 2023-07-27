namespace Freya.Features.Dialogs;

internal class ChangePassDialogViewModel : CoreViewModel
{
    private readonly IEmployeeAPIService employeeAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILogger<ChangePassDialogViewModel> logger;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterAnalitics appCenterAnalitics;

    private string actualPass = string.Empty;
    private string newPass = string.Empty;
    private string repeatNewPass = string.Empty;

    public ChangePassDialogViewModel(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<ChangePassDialogViewModel> logger,
        IDialogService dialogService,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        CancelCommand = new AsyncCommand(CancelCommandExecute);
        ChangePassCommand = new AsyncCommand(ChangePassCommandExecute, CanChangePass);
        this.taskHelperFactory = taskHelperFactory;
        this.employeeAPIService = refitService.InitRefitInstance<IEmployeeAPIService>(isAutenticated: true);
        this.logger = logger;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(ChangePassDialogViewModel));
    }

    private Task CancelCommandExecute()
    {
        dialogService.CloseAll();
        return Task.CompletedTask;  
    }

    private bool CanChangePass(object arg)
    {
        return !string.IsNullOrWhiteSpace(actualPass) && !string.IsNullOrWhiteSpace(newPass) && newPass == repeatNewPass;
    }

    private async Task ChangePassCommandExecute()
    {
        var request = new EmployeeChangePassRequest()
        {
            CurrentPass = this.actualPass,
            NewPass = this.NewPass,
        };

        IsBusy = true;
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnChangePassError).
                               TryExecuteAsync(
                               () => employeeAPIService.ChangePass(request));
        IsBusy = false;
        if (result.IsSuccess)
        {
            dialogService.CloseAll();
            await dialogService.ShowMessage(
                translatorService.Translate("dialog_client_profile_change_pass_success_title"),
                translatorService.Translate("dialog_client_profile_change_pass_success_body"));
        }
    }

    private async Task<bool> OnChangePassError(Exception arg)
    {
        if (arg is ApiException apiException && (apiException.StatusCode == System.Net.HttpStatusCode.NotFound))
        {
            await dialogService.ShowMessage(
                translatorService.Translate("error"),
                translatorService.Translate("check_fields"));
            return true;
        }

        return false;
    }

    public AsyncCommand ChangePassCommand { get; set; }

    public AsyncCommand CancelCommand { get; set; }

    public string ActualPass
    {
        get => actualPass; 
        set
        {
            SetAndRaisePropertyChanged(ref actualPass, value);
            ChangePassCommand?.RaiseCanExecuteChanged();
        }
    }
    public string NewPass
    {
        get => newPass; 
        set
        {
            SetAndRaisePropertyChanged(ref newPass, value);
            ChangePassCommand?.RaiseCanExecuteChanged();
        }
    }
    public string RepeatNewPass
    {
        get => repeatNewPass; 
        set
        {
            SetAndRaisePropertyChanged(ref repeatNewPass, value);
            ChangePassCommand?.RaiseCanExecuteChanged();
        }
    }
}
