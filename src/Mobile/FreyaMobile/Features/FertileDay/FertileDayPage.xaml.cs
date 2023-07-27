namespace FreyaMobile.Features.FertileDay;

public partial class FertileDayPage
{
	public FertileDayPage(FertileDayViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}