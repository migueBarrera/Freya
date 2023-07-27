using FreyaApi.Helpers;
using Models.Core.Employees;

namespace FreyaApi.Tests.Controllers.Services;

[TestFixture]
internal class EmployeeControllerServiceShould
{
    private ControllerBase controllerBase;
    private EmployeeControllerServiceBuilder builder;

    private string TEST_EMAIL = "test@gmail.com";
    private string TEST_PASS = "test";

    private string TEST_PASS_2 = "test2";

    [SetUp]
    public void SetUp()
    {
        builder = new EmployeeControllerServiceBuilder();
        controllerBase = new ControllerBaseBuilder().Build();
    }

    [Test]
    public async Task CanSignIn()
    {
        var service = builder
            .WithEmployee(TEST_EMAIL, Hash.Create(TEST_PASS))
            .Build();

        var request = new EmployeeSignInRequest()
        {
            Email = TEST_EMAIL,
            Pass = TEST_PASS,
        };

        var response = await service.PostSignIn(controllerBase, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
        });
    }

    [Test]
    public async Task SignInReturnNotFoundIfNotExist()
    {
        var service = builder
            .Build();

        var request = new EmployeeSignInRequest()
        {
            Email = TEST_EMAIL,
            Pass = TEST_PASS,
        };

        var response = await service.PostSignIn(controllerBase, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<NotFoundResult>());
        });
    }

    [Test]
    public async Task SignInReturnNotFoundIfPassDoesNotMatch()
    {
        var service = builder
            .WithEmployee(TEST_EMAIL, Hash.Create(TEST_PASS))
            .Build();

        var request = new EmployeeSignInRequest()
        {
            Email = TEST_EMAIL,
            Pass = TEST_PASS_2,
        };

        var response = await service.PostSignIn(controllerBase, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<NotFoundResult>());
        });
    }

    [Test]
    public async Task CanNotRegisterEmployeeCompanyOwnerToIndividualClinic()
    {
        var service = builder
            .WithEmployee(TEST_EMAIL, Hash.Create(TEST_PASS), EmployeeRol.COMPANY_OWNER)
            .Build();

        var request = new CheckEmployeeStateForRegisterResquest()
        {
            EmployeeEmail = TEST_EMAIL,
            ClinicId = Guid.NewGuid(),
        };

        var response = await service.CheckEmployeeStateForRegister(controllerBase, request);

        var resultObject = response.GetObjectResult();

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(resultObject.NewEmployeeState, Is.EqualTo(NewEmployeeState.CAN_NOT_REGISTER_COMPANY_EMPLOYEE_TO_INDIVIDUAL_CLINIC));
        });
    }
    
    [Test]
    public async Task CanNotRegisterAEmployeeWithClinicIdRequestEmpty()
    {
        var service = builder
            .WithEmployee(TEST_EMAIL, Hash.Create(TEST_PASS), EmployeeRol.CLINIC_MANAGER)
            .Build();

        var request = new CheckEmployeeStateForRegisterResquest()
        {
            EmployeeEmail = TEST_EMAIL,
            ClinicId = Guid.Empty,
        };

        var response = await service.CheckEmployeeStateForRegister(controllerBase, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<NotFoundResult>());
        });
    }
    
    [Test]
    public async Task CanNotRegisterAEmployeeWithREgisteredTootherClinicCompany()
    {
        Guid companyId = Guid.NewGuid();
        Guid companyId2 = Guid.NewGuid();

        var clinic1 = new ClinicTable()
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId
        };
        var clinic2 = new ClinicTable()
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId2
        };
        var clinics = new List<ClinicTable>()
        {
            clinic1
        };

        var service = builder
            .WithEmployeeWithClinic(TEST_EMAIL, Hash.Create(TEST_PASS), companyId, clinics, EmployeeRol.CLINIC_MANAGER)
            .AddClinic(clinic2)
            .Build();

        var request = new CheckEmployeeStateForRegisterResquest()
        {
            EmployeeEmail = TEST_EMAIL,
            ClinicId = clinic2.Id,
        };

        var response = await service.CheckEmployeeStateForRegister(controllerBase, request);

        var resultObject = response.GetObjectResult();

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(resultObject.NewEmployeeState, Is.EqualTo(NewEmployeeState.EMPLOYEE_REGISTERED_FOR_OTHER_COMPANY));
        });
    }
}
