namespace FreyaMobile.Features.Videos;

public partial class VideosPage
{
	public VideosPage(VideosViewModel viewModel)
	{
        BindingContext = viewModel;
		InitializeComponent();
	}
}