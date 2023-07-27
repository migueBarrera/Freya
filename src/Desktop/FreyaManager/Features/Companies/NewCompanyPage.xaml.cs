namespace FreyaManager.Features.Companies;

public partial class NewCompanyPage
{
    public NewCompanyPage(NewCompanyViewModel viewModel)
    {
        DataContext = viewModel;    
        InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}
