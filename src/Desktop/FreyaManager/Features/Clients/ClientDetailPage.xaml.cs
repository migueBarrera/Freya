namespace FreyaManager.Features.Clients;

public partial class ClientDetailPage
{
    public ClientDetailPage(ClientDetailViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {

    }
}
