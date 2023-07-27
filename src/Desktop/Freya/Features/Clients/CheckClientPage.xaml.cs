namespace Freya.Features.Clients.Services;

public partial class CheckClientPage 
{
    public CheckClientPage(CheckClientViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
