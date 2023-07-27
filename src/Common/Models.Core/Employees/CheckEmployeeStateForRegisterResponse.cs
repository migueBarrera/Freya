namespace Models.Core.Employees;

public class CheckEmployeeStateForRegisterResponse
{
    public CheckEmployeeStateForRegisterResponse(NewEmployeeState newEmployeeState)
    {
        NewEmployeeState = newEmployeeState;
    }

    public NewEmployeeState NewEmployeeState { get; set; }
}
