namespace Freya.Desktop.Core.Features.Clients;

public partial class EditClientPage
{
    public EditClientPage(EditClientViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
