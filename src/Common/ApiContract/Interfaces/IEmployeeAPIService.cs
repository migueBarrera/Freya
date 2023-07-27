using Models.Core.Clinics;
using Models.Core.Employees;

namespace ApiContract.Interfaces;

public interface IEmployeeAPIService
{
    [Post("/api/employee/v1/create")]
    Task<EmployeeSignUpResponse> CreateEmployeeAsync([Body] EmployeeSignUpRequest request);
    
    [Post("/api/employee/v1/signin")]
    Task<EmployeeSignInResponse> SignInAsync([Body] EmployeeSignInRequest request);

    [Put("/api/employee/v1")]
    Task UpdateEmployeeData([Body] EmployeeUpdateRequest request);
    
    [Get("/api/clinic/v1/{clinicId}/employees")]
    Task<PagedModel<EmployeeResponse>> GetEmployeesByClinicAsync(Guid clinicId, [Query] PaginationFilter PaginationFilter);
    
    [Get("/api/companies/v1/{companyID}/employees")]
    Task<PagedModel<EmployeeResponse>> GetEmployeesByCompanyAsync(Guid companyID, [Query] PaginationFilter PaginationFilter);
    
    [Post("/api/employee/v1/checkStateForRegister")]
    Task<CheckEmployeeStateForRegisterResponse> CheckEmployeeStateForRegister(CheckEmployeeStateForRegisterResquest request);
    
    [Post("/api/employee/v1/checkStateForRegisterForCompany")]
    Task<CheckEmployeeStateForRegisterResponse> CheckEmployeeStateForRegisterForACompany(CheckEmployeeStateForRegisterForCompanyResquest request);
    
    [Post("/api/employee/v1/assignToClinic")]
    Task AssignToClinic(AssignEmployeeToClinicRequest request);
    
    [Post("/api/employee/v1/unassignToClinic")]
    Task UnassignToClinic(UnassignEmployeeToClinicRequest request);
    
    [Get("/api/employee/v1/{employeeId}/getClinics")]
    Task<IEnumerable<ClinicResponse>> GetClinicsByEmployee(Guid employeeId);
    
    [Post("/api/employee/v1/changePass")]
    Task ChangePass(EmployeeChangePassRequest request);
    
    [Post("/api/employee/v1/recoverPass")]
    Task RecoverPass(EmployeeRecoverPassRequest request);
}
