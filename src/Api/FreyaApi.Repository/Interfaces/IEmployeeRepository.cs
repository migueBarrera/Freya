namespace FreyaApi.Repository.Interfaces;

public interface IEmployeeRepository
{
    Task<bool> ChangePassword(Guid userID, string newPassHashed);

    Task<EmployeeTable?> CreateEmployee(EmployeeSignUpRequest request, string pass, Guid clinicId);

    bool EmployeeExists(Guid id);

    bool EmployeeExists(string email);

    Task<EmployeeTable?> GetEmployee(Guid id);

    Task<EmployeeTable?> GetEmployee(string email);

    Task<EmployeeTable?> GetEmployeeWithClinicData(string email);

    Task<bool> ResetPassword(string email, string hashedPass);

    Task<bool> Update(EmployeeUpdateRequest request);

    IQueryable<EmployeeTable> GetAllEmployeeByCompanyId(Guid id, string? searchArgument = null);
}
