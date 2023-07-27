namespace FreyaMobile.Features.FertileDay;

public partial class FertileDayDetailPage
{
	public FertileDayDetailPage(FertileDayDetailViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}