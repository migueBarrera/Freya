namespace FreyaApi.Controllers.Services;

public class EmployeeControllerServices
{
    private readonly JwtSecurityTokenService jwtSecurityToken;
    private readonly IEmailService emailService;
    private readonly IEmployeeRepository employeeRepository;
    private readonly IClinicRepository clinicRepository;

    public EmployeeControllerServices(
        JwtSecurityTokenService jwtSecurityToken,
        IEmailService emailService,
        IEmployeeRepository employeeRepository,
        IClinicRepository clinicRepository)
    {
        this.jwtSecurityToken = jwtSecurityToken;
        this.emailService = emailService;
        this.employeeRepository = employeeRepository;
        this.clinicRepository = clinicRepository;
    }

    public async Task<ActionResult<EmployeeSignUpResponse>> CreateEmployee(ControllerBase controller, EmployeeSignUpRequest request)
    {
        if (employeeRepository.EmployeeExists(request.Email))
        {
            return controller.Conflict();
        }

        var pass = string.IsNullOrEmpty(request.Pass)
            ? PassGeneratorHelper.Generate()
            : request.Pass;

        EmployeeTable? employeeCreated = await employeeRepository.CreateEmployee(
            request,
            Hash.Create(pass),
            request.ClinicId);

        if (employeeCreated == null)
        {
            return controller.NotFound();
        }

        emailService.SendMailWelcomeEmployee(request.Email,request.Name, pass);

        var response = new EmployeeSignUpResponse()
        {
            Id = employeeCreated.Id,
            Email = employeeCreated.Email,
            Name = employeeCreated.Name,
            LastName = employeeCreated.LastName,
            Rol = employeeCreated.Rol,
            Clinics = Enumerable.Empty<ClinicResponse>(),
            Token = string.Empty,
            RefrehToken = string.Empty,
        };

        return controller.Created("GetEmployee", response);
    }

    public async Task<ActionResult<EmployeeSignInResponse>> PostSignIn(ControllerBase controller, EmployeeSignInRequest request)
    {
        EmployeeTable? employee = await employeeRepository.GetEmployee(request.Email);

        if (employee == null)
        {
            return controller.NotFound();
        }

        if (!Hash.Validate(request.Pass, employee.Pass))
        {
            return controller.NotFound();
        }

        List<ClinicResponse> clinicsResponse = await GetClinicsByEmployeeRolWithSubscriptionData(employee.Id);

        var response = new EmployeeSignInResponse()
        {
            Id = employee.Id,
            Name = employee.Name,
            Rol = employee.Rol,
            LastName = employee.LastName,
            CompanyId = employee.CompanyId,
            Email = employee.Email,
            Clinics = clinicsResponse,
            Token = jwtSecurityToken.BuildToken(employee),
            RefreshToken = jwtSecurityToken.BuildRefreshToken(employee),
        };

        return controller.Ok(response);
    }

    public async Task<ActionResult> UpdateEmployee(ControllerBase controller, EmployeeUpdateRequest request)
    {
        bool updated = await employeeRepository.Update(request);
        return controller.Ok();
    }

    public async Task<ActionResult> ChangePassword(ControllerBase controller, EmployeeChangePassRequest request)
    {
        var userID = controller.GetLoggedUserId();
        var newPassHashed = Hash.Create(request.NewPass);
        EmployeeTable? employee = await employeeRepository.GetEmployee(userID);
        if (employee == null)
        {
            return controller.NotFound();
        }

        if (!Hash.Validate(request.CurrentPass, employee.Pass))
        {
            return controller.NotFound();
        }

        bool changed = await employeeRepository.ChangePassword(userID, newPassHashed);
        if (!changed)
        {
            return controller.NotFound();
        }

        return controller.Ok();
    }

    public async Task<ActionResult> RecoverPassword(ControllerBase controller, string email)
    {
        if (!employeeRepository.EmployeeExists(email))
        {
            return controller.NotFound();
        }
        var newPass = PassGeneratorHelper.Generate();
        bool reseted = await employeeRepository.ResetPassword(email, Hash.Create(newPass));

        if (!reseted)
        {
            return controller.NotFound();
        }

        emailService.SendMailChangePassEmployee(email, newPass);

        return controller.Ok();
    }

    public async Task<ActionResult<CheckEmployeeStateForRegisterResponse>> CheckEmployeeStateForRegister(ControllerBase controller, CheckEmployeeStateForRegisterResquest request)
    {
        if (request.ClinicId == Guid.Empty || string.IsNullOrEmpty(request.EmployeeEmail))
        {
            return controller.NotFound();
        }

        if (!employeeRepository.EmployeeExists(request.EmployeeEmail))
        {
            return controller.Ok(new CheckEmployeeStateForRegisterResponse(NewEmployeeState.EMPLOYEE_NOT_REGISTERED));
        }

        EmployeeTable? employee = await employeeRepository.GetEmployeeWithClinicData(request.EmployeeEmail);
        if (employee == null)
        {
            return controller.NotFound();
        }

        if (employee.Rol == EmployeeRol.COMPANY_MANAGER || employee.Rol == EmployeeRol.COMPANY_OWNER)
        {
            return controller.Ok(new CheckEmployeeStateForRegisterResponse(NewEmployeeState.CAN_NOT_REGISTER_COMPANY_EMPLOYEE_TO_INDIVIDUAL_CLINIC));
        }

        var employeeIsAsignedForClinic = employee.Clinics.Any(c => c.Id == request.ClinicId);
        if (employeeIsAsignedForClinic)
        {
            return controller.Ok(new CheckEmployeeStateForRegisterResponse(NewEmployeeState.EMPLOYEE_REGISTERED_AND_ASIGNED));
        }

        var clinic = clinicRepository.GetClinic(request.ClinicId);

        if (clinic == null)
        {
            return controller.NotFound();
        }

        if (employee.CompanyId != clinic.CompanyId)
        {
            return controller.Ok(new CheckEmployeeStateForRegisterResponse(NewEmployeeState.EMPLOYEE_REGISTERED_FOR_OTHER_COMPANY));
        }

        return controller.Ok(new CheckEmployeeStateForRegisterResponse(NewEmployeeState.EMPLOYEE_REGISTERED_BUT_NOT_ASIGNED));
    }
    
    public async Task<ActionResult<CheckEmployeeStateForRegisterResponse>> CheckEmployeeStateForRegisterForCompany(ControllerBase controller, CheckEmployeeStateForRegisterForCompanyResquest request)
    {
        if (!employeeRepository.EmployeeExists(request.EmployeeEmail))
        {
            return controller.Ok(new CheckEmployeeStateForRegisterResponse(NewEmployeeState.EMPLOYEE_NOT_REGISTERED));
        }

        EmployeeTable? employee = await employeeRepository.GetEmployeeWithClinicData(request.EmployeeEmail);
        if (employee == null)
        {
            return controller.NotFound();
        }

        if (employee.CompanyId != request.ConpanyId)
        {
            return controller.Ok(new CheckEmployeeStateForRegisterResponse(NewEmployeeState.EMPLOYEE_REGISTERED_FOR_OTHER_COMPANY));
        }

        return controller.Ok(new CheckEmployeeStateForRegisterResponse(NewEmployeeState.EMPLOYEE_REGISTERED_AND_ASIGNED));
    }

    public async Task<IActionResult> AssignToClinic(ControllerBase controller, AssignEmployeeToClinicRequest request)
    {
        if (!employeeRepository.EmployeeExists(request.EmployeeEmail))
        {
            return controller.NotFound();
        }

        var added = await clinicRepository.AddEmployeeToClinic(request.ClinicId, request.EmployeeEmail);

        return added ?
            controller.Ok() :
            controller.NotFound();
    }

    public async Task<IActionResult> UnAssignToClinic(ControllerBase controller, UnassignEmployeeToClinicRequest request)
    {
        //TODO comprobar rol superior de quien quiere eliminar al empleado
        if (!employeeRepository.EmployeeExists(request.EmployeeId))
        {
            return controller.NotFound();
        }

        bool unassingned = await clinicRepository.RemoveEmployeeFromClinic(request.ClinicId, request.EmployeeId);

        return unassingned ?
            controller.Ok() :
            controller.NotFound();
    }

    public async Task<ActionResult<IEnumerable<ClinicResponse>>> GetClinicsFromEmployeeByRol(ControllerBase controller, Guid id)
    {
        if (!employeeRepository.EmployeeExists(id))
        {
            return controller.NotFound();
        }

        List<ClinicResponse> clinicsResponse = await GetClinicsByEmployeeRolWithSubscriptionData(id);

        return controller.Ok(clinicsResponse);
    }

    private async Task<List<ClinicResponse>> GetClinicsByEmployeeRolWithSubscriptionData(Guid id)
    {
        var clinicsResponse = new List<ClinicResponse>();

        IEnumerable<ClinicTable> clinics = await clinicRepository.GetAllClinicFromEmployeeByRol(id);

        foreach (var clinic in clinics)
        {
            var currentSubcription = clinic.SubscriptionPayments.FirstOrDefault(x => x.State == SubscriptionStates.ACTIVE);

            var clinicResponse = new ClinicResponse()
            {
                Id = clinic.Id,
                Name = clinic.Name,
                CompanyId = clinic.CompanyId,
                CurrentSubscription = currentSubcription != null
                    ? SubscriptionsMapper.ConverTo(currentSubcription)
                    : null,
            };
            clinicsResponse.Add(clinicResponse);
        }

        return clinicsResponse;
    }
}
