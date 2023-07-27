namespace FreyaManager.Features.Clinics;

public partial class CreateClinicPage
{
    public CreateClinicPage(CreateClinicViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}
