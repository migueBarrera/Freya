namespace FreyaManager.Features.Companies;

public partial class DetailCompanyPage
{
    public DetailCompanyPage(DetailCompanyViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}
