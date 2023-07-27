namespace Freya.Features.Profile;

public class ProfileViewModel : CoreViewModel
{
    private readonly INavigationService navigationService;
    private readonly ICurrentEmployeeService currentEmployeeService;
    private readonly IDialogService dialogService;
    private readonly ProfileService profileService;
    private ProfileModel profileModel;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterAnalitics appCenterAnalitics;

    public ProfileViewModel(
        INavigationService navigationService,
        ICurrentEmployeeService currentEmployeeService,
        ProfileService profileService,
        IDialogService dialogService,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        ShowBackButton = true;
        Title = translatorService.Translate("page_title_profile");
        this.navigationService = navigationService;
        profileModel = new ProfileModel();
        SaveChangesProfileCommand = new AsyncCommand(SaveChangesProfileCommandExecute);
        ChangePassCommand = new AsyncCommand(ChangePassCommandExecute);
        BackCommand = new AsyncCommand(BackCommandExecute);
        this.currentEmployeeService = currentEmployeeService;
        this.profileService = profileService;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(ProfileViewModel));
    }

    private Task ChangePassCommandExecute()
    {
        return profileService.ChangePass();
    }

    public ProfileModel ProfileModel { get => profileModel; set => SetAndRaisePropertyChanged(ref profileModel, value); }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        var employee = currentEmployeeService.Employee ?? new Employee();
        ProfileModel.EmployeeEmail = employee.Email;
        ProfileModel.EmployeeName = employee.Name;
        ProfileModel.EmployeeLastName = employee.LastName;

        return base.OnAppearingAsync(parameter);
    }

    private async Task SaveChangesProfileCommandExecute()
    {
        if (string.IsNullOrWhiteSpace(ProfileModel.EmployeeName)
            || !EmailValidatorService.IsValidEmail(ProfileModel.EmployeeEmail))
        {
            await dialogService.ShowMessage(
                translatorService.Translate("error"),
                translatorService.Translate("check_fields"));
            return;
        }

        appCenterAnalitics.Clicked("Save profile data");
        var result = await profileService.SaveChanges(ProfileModel.EmployeeName, ProfileModel.EmployeeLastName);
        if (result)
        {
            if (Parent is MainViewModel viewModel)
            {
                await viewModel.LoadItems();
            }
            await dialogService.ShowMessage(
                translatorService.Translate("dialog_profile_updated_success_title"),
                translatorService.Translate("dialog_profile_updated_success_body"));
            await navigationService.BackAsync();
        }
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    public IAsyncCommand BackCommand { get; set; }

    public IAsyncCommand ChangePassCommand { get; set; }

    public IAsyncCommand SaveChangesProfileCommand { get; set; }
}
