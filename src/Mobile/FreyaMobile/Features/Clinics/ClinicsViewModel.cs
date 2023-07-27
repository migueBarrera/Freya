namespace FreyaMobile.Features.Clinics;

public class ClinicsViewModel : CoreViewModel
{
    private readonly ICurrentUserService currentUserService;

    private bool isLogged;
    private bool hasClientData;

    public ClinicsViewModel(
        ClinicNotLoggedViewModel clinicNotLoggedViewModel,
        ClinicLoggedWithOutClinicDataViewModel clinicLoggedWithOutClinicDataViewModel,
        ClinicLoggedWithClinicDataViewModel clinicLoggedWithClinicDataViewModel,
        ICurrentUserService currentUserService)
    {
        this.ClinicNotLoggedViewModel = clinicNotLoggedViewModel;
        this.ClinicLoggedWithOutClinicDataViewModel = clinicLoggedWithOutClinicDataViewModel;
        this.ClinicLoggedWithClinicDataViewModel = clinicLoggedWithClinicDataViewModel;

        this.ClinicLoggedWithOutClinicDataViewModel.Parent = this;

        this.currentUserService = currentUserService;
    }

    public ClinicNotLoggedViewModel ClinicNotLoggedViewModel { get; private set; }

    public ClinicLoggedWithOutClinicDataViewModel ClinicLoggedWithOutClinicDataViewModel { get; private set; }

    public ClinicLoggedWithClinicDataViewModel ClinicLoggedWithClinicDataViewModel { get; private set; }

    public bool IsLogged { get => isLogged; set => SetAndRaisePropertyChanged(ref isLogged, value); }

    public bool HasClientData { get => hasClientData; set => SetAndRaisePropertyChanged(ref hasClientData, value); }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        await LoadStatesAsync();
        await EnteringViewModelAsync();
    }

    private async Task LoadStatesAsync()
    {
        IsLogged = await currentUserService.IsLoggedAsync();
        if (isLogged)
        {
            HasClientData = await currentUserService.HasClientDataAsync();
        }
    }

    private async Task EnteringViewModelAsync()
    {
        if (!isLogged)
        {
            await ClinicNotLoggedViewModel.OnAppearingAsync();
        }
        else
        {
            if (hasClientData)
            {
                await ClinicLoggedWithClinicDataViewModel.OnAppearingAsync();
            }
            else
            {
                await ClinicLoggedWithOutClinicDataViewModel.OnAppearingAsync();
            }
        }
    }
}
