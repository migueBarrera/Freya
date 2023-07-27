namespace Freya.Features.Clients;

public partial class ClientsPage
{
    public ClientsPage(ClientsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
