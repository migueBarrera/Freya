using Freya.Desktop.Core.Framework;
using Models.Core.Clinics;
using Models.Core.Employees;

namespace Freya.Tests.Features.Clinics;

[TestFixture]
public class ClinicDetailViewModelShould
{
    private ClinicDetailViewModelBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new ClinicDetailViewModelBuilder();
    }

    [Test]
    public async Task ClinicDetail_Expected_A_Clinic_Id_From_Sesion()
    {
        var clinicId = Guid.NewGuid();  
        var viewModel = builder
            .WithRol(EmployeeRol.COMPANY_OWNER)
            .SaveOnSession<Guid>(SESSION.KEY_CLINIC_ID_SELECTED, clinicId)
            .WithClinic(new ClinicDetailResponse())
            .Build();

        await viewModel.OnAppearingAsync(null);

        builder.clinicDetailService.Verify(x => x.GetClinic(clinicId), Times.Once());
    }

    [Test]
    public void Go_To_Add_Client_Require_Add_Clinic_Id_To_Session()
    {
        var viewModel = builder.Build();

        var id = Guid.NewGuid();
        viewModel.Clinic = new ClinicDetailResponse() { Id = id,};
        viewModel.AddClientCommand.Execute(null);

        builder.sessionService.Verify(x => 
            x.Save(It.Is<string>(x => x == SESSION.KEY_CLINIC_ID_SELECTED), It.Is<Guid>(x => x == id)), 
            Times.Once);
    }
    
    [Test]
    public void Go_To_Add_Employee_Require_Add_Clinic_Id_To_Session()
    {
        var viewModel = builder.Build();

        var id = Guid.NewGuid();
        viewModel.Clinic = new ClinicDetailResponse() { Id = id,};
        viewModel.AddEmployeeCommand.Execute(null);

        builder.sessionService.Verify(x => 
            x.Save(It.Is<string>(x => x == SESSION.KEY_CLINIC_ID_SELECTED), It.Is<Guid>(x => x == id)), 
            Times.Once);
    }

    [Test]
    [TestCase(EmployeeRol.COMPANY_MANAGER, true)]
    [TestCase(EmployeeRol.COMPANY_OWNER, true)]
    [TestCase(EmployeeRol.CLINIC_MANAGER, false)]
    [TestCase(EmployeeRol.CLINIC_OFFICER, false)]
    public async Task IsCompanyAdminIsTrueIfCorrespond(EmployeeRol employeeRol, bool expectedIsCompanyAdmin)
    {
        var viewModel = builder
            .WithRol(employeeRol)
            .WithClinic(new ClinicDetailResponse())
            .Build();

        await viewModel.OnAppearingAsync(null);

        Assert.That(expectedIsCompanyAdmin, Is.EqualTo(viewModel.IsCompanyAdmin));   
    }
    
    [Test]
    [TestCase(EmployeeRol.COMPANY_MANAGER, false)]
    [TestCase(EmployeeRol.COMPANY_OWNER, false)]
    [TestCase(EmployeeRol.CLINIC_MANAGER, false)]
    [TestCase(EmployeeRol.CLINIC_OFFICER, true)]
    public async Task IsClinicOfficerTrueIfCorrespond(EmployeeRol employeeRol, bool expectedIsOfficeOffcier)
    {
        var viewModel = builder
            .WithRol(employeeRol)
            .WithClinic(new ClinicDetailResponse())
            .Build();

        await viewModel.OnAppearingAsync(null);

        Assert.That(expectedIsOfficeOffcier, Is.EqualTo(viewModel.IsClinicOfficer));   
    }
}
