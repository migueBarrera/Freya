namespace FreyaMobile.Features.Clients.Register;

public partial class RegisterPage
{
	public RegisterPage(RegisterViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}