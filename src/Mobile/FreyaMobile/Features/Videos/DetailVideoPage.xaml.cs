namespace FreyaMobile.Features.Videos;

public partial class DetailVideoPage
{
	public DetailVideoPage(DetailVideoViewModel viewModel)
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