namespace Freya.Features.Employees;

public partial class CreateNewEmployeePage
{
    public CreateNewEmployeePage(CreateNewEmployeeViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
