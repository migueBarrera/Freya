namespace FreyaManager.Features.Employees;

public partial class CreateEmployeePage
{
    public CreateEmployeePage(CreateEmployeeViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}
