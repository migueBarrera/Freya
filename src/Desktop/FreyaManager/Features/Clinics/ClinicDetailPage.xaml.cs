namespace FreyaManager.Features.Clinics;

public partial class ClinicDetailPage
{
    public ClinicDetailPage(ClinicDetailViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}
