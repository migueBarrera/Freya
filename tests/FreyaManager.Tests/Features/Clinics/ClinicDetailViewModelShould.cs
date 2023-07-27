namespace FreyaManager.Tests.Features.Clinics;

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
        ClinicResponse clinicResponse = new ClinicResponse() { Id = clinicId,};
        ClinicDetailResponse clinicDetailResponseinicResponse = new ClinicDetailResponse() { Id = clinicId,};
        var viewModel = builder
            .SaveOnSession<ClinicResponse>(SESSION.KEY_CLINIC_SELECTED, clinicResponse)
            .WithClinic(clinicDetailResponseinicResponse)
            .Build();

        await viewModel.OnAppearingAsync(null);

        builder.clinicService.Verify(x => x.GetClinic(clinicId), Times.Once());
    }

    [Test]
    public void Go_To_Edit_Client_Require_Add_Clinic_Id_To_Session()
    {
        var viewModel = builder.Build();

        var id = Guid.NewGuid();
        viewModel.Clinic = new ClinicDetailResponse() { Id = id, };
        viewModel.EditClinicCommand.Execute(null);

        builder.sessionService.Verify(x =>
            x.Save(It.Is<string>(x => x == SESSION.KEY_CLINIC_DETAIL_SELECTED), It.Is<ClinicDetailResponse>(x => x.Id == id)),
            Times.Once);
    }

    [Test]
    public void Go_To_Add_Employee_Require_Add_Clinic_Id_To_Session()
    {
        var viewModel = builder.Build();

        var id = Guid.NewGuid();
        viewModel.Clinic = new ClinicDetailResponse() { Id = id, };
        viewModel.CreateEmployeeCommand.Execute(null);

        builder.sessionService.Verify(x =>
            x.Save(It.Is<string>(x => x == SESSION.KEY_NEW_EMPLOYEE_FOR_CLINIC_ID), It.Is<Guid>(x => x == id)),
            Times.Once);
        
        builder.sessionService.Verify(x =>
            x.Save(It.Is<string>(x => x == SESSION.KEY_NEW_EMPLOYEE_FOR_CLINIC), It.Is<bool>(x => x == true)),
            Times.Once);
    }
}
