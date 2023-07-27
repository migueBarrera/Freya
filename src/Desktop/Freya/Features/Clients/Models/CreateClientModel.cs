namespace Freya.Features.Clients.Models;

public class CreateClientModel : ObservableObject
{
    private string employeeName = string.Empty;
    private string employeeEmail = string.Empty;
    private string employeeLastName = string.Empty;
    private string employeePhone = string.Empty;

    public string EmployeeName { get => employeeName; set => SetAndRaisePropertyChanged(ref employeeName, value); }
    public string EmployeeEmail { get => employeeEmail; set => SetAndRaisePropertyChanged(ref employeeEmail, value); }
    public string EmployeePhone { get => employeePhone; set => SetAndRaisePropertyChanged(ref employeePhone, value); }
    public string EmployeeLastName { get => employeeLastName; set => SetAndRaisePropertyChanged(ref employeeLastName, value); }

    internal bool Validate()
    {
        return !string.IsNullOrEmpty(EmployeeEmail) || !string.IsNullOrWhiteSpace(EmployeeName);
    }
}
