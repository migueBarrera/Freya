using FreyaMobile.Core.Framework;

namespace FreyaMobile.Features.HelpAndInfo.Models;

public class HAIMenuItem : ObservableObject
{
    public string Title { get; set; } = string.Empty;

    public  ImageSource? Image { get; set; }

    public TypeHAIMenu Type { get; set; }

    private bool isEnabled;
    public bool IsEnabled
    { 
        get => isEnabled; 
        set => SetAndRaisePropertyChanged(ref isEnabled, value); 
    }
}
