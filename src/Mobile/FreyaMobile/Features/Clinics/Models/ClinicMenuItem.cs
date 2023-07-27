namespace FreyaMobile.Features.Clinics.Models;

public class ClinicMenuItem : ObservableObject
{
    public string Title { get; set; } = string.Empty;

    public ImageSource? Image { get; set; }

    public TypeClinicMenu Type { get; set; }

    private bool isEnabled = true;
    public bool IsEnabled
    {
        get => isEnabled;
        set => SetAndRaisePropertyChanged(ref isEnabled, value);
    }
}
