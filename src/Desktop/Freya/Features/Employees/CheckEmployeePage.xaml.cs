namespace Freya.Features.Employees;

public partial class CheckEmployeePage 
{
    public CheckEmployeePage(CheckEmployeeViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
