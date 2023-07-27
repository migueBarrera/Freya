namespace FreyaMobile.Features.Clients;

public partial class EditClientPage
{
	public EditClientPage(EditClientViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}