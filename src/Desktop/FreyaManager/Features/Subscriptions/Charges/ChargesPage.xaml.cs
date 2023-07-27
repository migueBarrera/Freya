namespace FreyaManager.Features.Subscriptions.Charges;

public partial class ChargesPage
{
    public ChargesPage(ChargesViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
