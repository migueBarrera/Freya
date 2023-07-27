namespace Freya.Features.Employees;

public partial class EditEmployeePage 
{
    public EditEmployeePage(EditEmployeeViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
