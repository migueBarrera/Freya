namespace Freya.Features.Employees.Models;

public class CreateEmployeeModel : ObservableObject
{
    private EmployeeRol rolSelected;
    private string employeeName = string.Empty;
    private string employeeEmail = string.Empty;
    private string employeeLastName = string.Empty;
    private IEnumerable<CapsuleRol> rolList = Enumerable.Empty<CapsuleRol>();

    public string EmployeeName { get => employeeName; set => SetAndRaisePropertyChanged(ref employeeName, value); }
    public string EmployeeEmail { get => employeeEmail; set => SetAndRaisePropertyChanged(ref employeeEmail!, value); }
    public string EmployeeLastName { get => employeeLastName; set => SetAndRaisePropertyChanged(ref employeeLastName!, value); }
    public IEnumerable<CapsuleRol> RolList { get => rolList; set => SetAndRaisePropertyChanged(ref rolList, value); }
    public EmployeeRol RolSelected { get => rolSelected; set => SetAndRaisePropertyChanged(ref rolSelected, value); }
}

public class CapsuleRol
{
    public string Text { get; set; } = string.Empty;
    public EmployeeRol Rol { get; set; }
}
