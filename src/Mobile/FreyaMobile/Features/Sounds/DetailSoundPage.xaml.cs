
namespace FreyaMobile.Features.Sounds;

public partial class DetailSoundPage
{
	public DetailSoundPage(DetailSoundViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MediaElementControl.Stop();
    }
}