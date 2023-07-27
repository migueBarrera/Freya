namespace FreyaMobile.Features.Clients.ForgotPass;

public partial class ForgotPassPage
{
	public ForgotPassPage(ForgotPassViewModel viewModel)
	{
		BindingContext = viewModel;
		viewModel.Popup = this;
		InitializeComponent();
	}
}