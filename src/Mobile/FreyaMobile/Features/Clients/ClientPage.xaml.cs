namespace FreyaMobile.Features.Clients;

public partial class ClientPage
{
	public ClientPage(ClientViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}