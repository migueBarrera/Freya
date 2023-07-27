using Models.Core.Employees;

namespace Freya.Tests.Features.Employees;

[TestFixture]
public class CreateNewEmployeeViewModelShould
{
    private CreateNewEmployeeViewModelBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new CreateNewEmployeeViewModelBuilder();
    }

    [Test]
    public async Task Employee_With_Rol_ClinicOfficer_Only_Can_Create_Employee_With_Rol_ClinicOfficer()
    {
        var viewModel = builder
            .WithRol(EmployeeRol.CLINIC_OFFICER)
            .Build();

        await viewModel.OnAppearingAsync();

        var rolList = viewModel.CreateEmployeeModel.RolList.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(rolList.Count(), Is.EqualTo(1));
            Assert.That(rolList.First().Rol, Is.EqualTo(EmployeeRol.CLINIC_OFFICER));
        });
    }
    
    [Test]
    [TestCase(EmployeeRol.CLINIC_MANAGER)]
    [TestCase(EmployeeRol.COMPANY_MANAGER)]
    [TestCase(EmployeeRol.COMPANY_OWNER)]
    public async Task Employee_With_Rol_ClinicOfficer_Or_Higer_Can_Create_Employee_With_Rol_ClinicOfficer_And_ClinicManager(EmployeeRol employeeRol)
    {
        var viewModel = builder
            .WithRol(employeeRol)
            .Build();

        await viewModel.OnAppearingAsync();

        var rolList = viewModel.CreateEmployeeModel.RolList.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(rolList.Count(), Is.EqualTo(2));
            Assert.That(rolList.Where(x => x.Rol == EmployeeRol.CLINIC_OFFICER), Is.Not.Null);
            Assert.That(rolList.Where(x => x.Rol == EmployeeRol.CLINIC_MANAGER), Is.Not.Null);
        });
    }
}
