namespace Freya.Tests.Features.Clinics;

[TestFixture]
public class CreateNewClinicViewModelShould
{
    private CreateNewClinicViewModelBuilder builder;

    [SetUp]
    public void Setup()
    {
        builder = new CreateNewClinicViewModelBuilder();
    }

    [Test]
    public async Task CreateNewClinic_Expected_companyId_From_Current_Employee()
    {
        var employee = new Models.Core.Employees.Employee()
        {
            CompanyId = Guid.NewGuid(),
        };

        var viewModel = builder
            .WithEmployee(employee)
            .Build();

        await viewModel.OnAppearingAsync();
    }
}
