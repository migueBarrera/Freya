namespace FreyaMobile.Features.Images;

public partial class ImagesPage 
{
	public ImagesPage(ImagesViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}