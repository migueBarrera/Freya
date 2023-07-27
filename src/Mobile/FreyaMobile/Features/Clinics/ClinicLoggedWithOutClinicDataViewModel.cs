namespace FreyaMobile.Features.Clinics;

public class ClinicLoggedWithOutClinicDataViewModel : CoreViewModel
{
    private readonly ICurrentUserService currentUserService;
    private readonly IUserService userService;
    private readonly DialogService dialogService;

    public ClinicLoggedWithOutClinicDataViewModel(
        IUserService userService,
        DialogService dialogService,
        ICurrentUserService currentUserService)
    {
        this.userService = userService;

        this.CheckCommand = new AsyncCommand(CheckAsync);
        this.dialogService = dialogService;
        this.currentUserService = currentUserService;
    }

    public ClinicsViewModel? Parent { get; set; }

    public IAsyncCommand CheckCommand { get; set; }

    private async Task CheckAsync()
    {
        var result = await userService.TryRefreshUserAsync();
        if (result.IsSuccess)
        {
            if (await currentUserService.HasClientDataAsync())
            {
                await Parent!.OnAppearingAsync();
            }
            else
            {
                await dialogService.DisplayAlert("Parece que sigues sin ninguna clínica asociada.", string.Empty);
            }
        }
    }
}
