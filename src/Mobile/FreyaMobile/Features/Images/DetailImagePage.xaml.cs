namespace FreyaMobile.Features.Images;

public partial class DetailImagePage
{
	public DetailImagePage(DetailImageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}