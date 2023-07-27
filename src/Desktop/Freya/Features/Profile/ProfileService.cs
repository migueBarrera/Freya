namespace Freya.Features.Profile;

public class ProfileService
{
    private readonly IDialogService dialogService;
    private readonly IEditEmployeeService editEmployeeService;
    private readonly ICurrentEmployeeService currentEmployeeService;
    private readonly INavigationService navigationService;

    public ProfileService(
        IDialogService dialogService,
        IEditEmployeeService editEmployeeService,
        ICurrentEmployeeService currentEmployeeService,
        INavigationService navigationService)
    {
        this.dialogService = dialogService;
        this.editEmployeeService = editEmployeeService;
        this.currentEmployeeService = currentEmployeeService;
        this.navigationService = navigationService;
    }

    internal Task ChangePass()
    {
        return dialogService.ShowDialog<ChangePassDialog, ChangePassDialogViewModel>();
    }

    internal async Task<bool> SaveChanges(string name, string lastName)
    {
        var currentEmployee = currentEmployeeService.Employee ?? new Employee();
        var result = await editEmployeeService.SaveEmployeeData(new EmployeeUpdateRequest()
        {
            Name = name,
            LastName = lastName,
            Id = currentEmployee.Id,
        });


        if (result)
        {
            currentEmployee.Name = name;
            currentEmployee.LastName = lastName;
            await currentEmployeeService.SetEmployee(currentEmployee);
        }

        return result;
    }
}
