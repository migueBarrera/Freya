namespace FreyaMobile.Features.Clients.ChangePass;

public partial class ChangePassPage
{
	public ChangePassPage(ChangePassViewModel viewModel)
	{
		BindingContext = viewModel;
		viewModel.Popup = this;
		InitializeComponent();
	}
}