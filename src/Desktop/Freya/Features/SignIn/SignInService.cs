using Freya.Desktop.Core.Helpers;

namespace Freya.Features.SignIn;

public class SignInService : ISignInService
{
    private readonly IEmployeeAPIService employeeAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly IAuthTokenService authTokenService;
    private readonly ICurrentEmployeeService currentEmployeeService;
    private readonly IEmployeeRolService employeeRolService;
    private readonly ITranslatorService translatorService;
    private readonly ICurrentClinicService currentClinicService;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private readonly ILogger<SignInService> logger;

    public SignInService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<SignInService> logger,
        IDialogService dialogService,
        IAuthTokenService authTokenService,
        ICurrentEmployeeService currentEmployeeService,
        IEmployeeRolService employeeRolService,
        ITranslatorService translatorService,
        ICurrentClinicService currentClinicService,
        AppCenterAnalitics appCenterAnalitics)
    {
        employeeAPIService = refitService.InitRefitInstance<IEmployeeAPIService>();
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
        this.dialogService = dialogService;
        this.authTokenService = authTokenService;
        this.currentEmployeeService = currentEmployeeService;
        this.employeeRolService = employeeRolService;
        this.translatorService = translatorService;
        this.currentClinicService = currentClinicService;
        this.appCenterAnalitics = appCenterAnalitics;
    }

    public async Task<bool> RecoverPass(string email)
    {
        var result = await taskHelperFactory.
                                CreateInternetAccessViewModelInstance(logger).
                                WithErrorHandling(OnSignInError).
                                TryExecuteAsync(
                                () => employeeAPIService.RecoverPass(new EmployeeRecoverPassRequest()
                                {
                                    Email = email,
                                }));
        return result.IsSuccess;
    }

    public async Task<bool> SignInAsync(string email, string pass)
    {
        var resultIsSucces = false;
        var result = await taskHelperFactory.
                                CreateInternetAccessViewModelInstance(logger).
                                WithErrorHandling(OnSignInError).
                                TryExecuteAsync(
                                () => employeeAPIService.SignInAsync(new EmployeeSignInRequest()
                                {
                                    Email = email,
                                    Pass = pass,
                                }));
        if (result.IsSuccess)
        {
            var employee = result.Value;
            appCenterAnalitics.SignInData(employee.CompanyId, translatorService.GetTextByRol(employee.Rol));
            bool employeeNotIsManagerAndNoHasAnyClinic = employeeRolService.IsClinicManagerOrMinor(employee.Rol) && !employee.Clinics.Any();
            if (employeeNotIsManagerAndNoHasAnyClinic)
            {
                await dialogService.ShowMessage(
                    translatorService.Translate("dialog_employee_not_is_manager_and_not_has_any_clinic_title"),
                    translatorService.Translate("dialog_employee_not_is_manager_and_not_has_any_clinic_body"));
            }
            else
            {
                await currentClinicService.Init(employee.Clinics.ToList());
                await currentEmployeeService.SetEmployee(new Employee()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    LastName = employee.LastName,
                    CompanyId = employee.CompanyId,
                    Rol = employee.Rol,
                    Email = employee.Email,
                });
                await authTokenService.SetToken(employee.Token, employee.RefreshToken);
                resultIsSucces = true;
            }
        }

        return resultIsSucces;
    }

    private async Task<bool> OnSignInError(Exception arg)
    {
        if (arg is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await dialogService.ShowMessage(
                translatorService.Translate("error"),
                translatorService.Translate("check_fields"));
            return true;
        }

        return false;
    }
}
