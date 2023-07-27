namespace FreyaMobile.Features.Sounds;

public partial class SoundsPage
{
	public SoundsPage(SoundsViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
	}
}