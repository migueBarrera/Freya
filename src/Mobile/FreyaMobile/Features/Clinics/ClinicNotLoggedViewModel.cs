namespace FreyaMobile.Features.Clinics;

public class ClinicNotLoggedViewModel : CoreViewModel
{
    public ClinicNotLoggedViewModel()
    {
        this.GoToLoginCommand = new AsyncCommand(GoToLoginAsync);
    }

    public IAsyncCommand GoToLoginCommand { get; set; }

    private async Task GoToLoginAsync()
    {
        await Shell.Current.GoToAsync($"//{nameof(ClientPage)}");
    }
}
