namespace Freya.Features.Clients;

public partial class CreateNewClientPage 
{
    public CreateNewClientPage(CreateNewClientViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
