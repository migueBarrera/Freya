namespace Freya.Features.Employees.Models;

public class EditEmployeeModel : ObservableObject
{
    private string employeeName = string.Empty;
    private string employeeEmail = string.Empty;
    private string employeeLastName = string.Empty;

    public string EmployeeName { get => employeeName; set => SetAndRaisePropertyChanged(ref employeeName, value); }
    public string EmployeeEmail { get => employeeEmail; set => SetAndRaisePropertyChanged(ref employeeEmail, value); }
    public string EmployeeLastName { get => employeeLastName; set => SetAndRaisePropertyChanged(ref employeeLastName, value); }
}
