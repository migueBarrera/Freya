namespace FreyaMobile.Features.HelpAndInfo;

public partial class HelpAndInfoPage
{
	public HelpAndInfoPage(HelpAndInfoViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}