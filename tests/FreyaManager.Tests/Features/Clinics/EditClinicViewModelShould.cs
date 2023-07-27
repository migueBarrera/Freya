namespace FreyaManager.Tests.Features.Clinics;

[TestFixture]
public class EditClinicViewModelShould
{
    private EditClinicViewModelBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new EditClinicViewModelBuilder();
    }

    [Test]
    public async Task EditClinic_Expect_ClinicDetailSelected_From_Session()
    {
        var id = Guid.NewGuid();
        ClinicDetailResponse clinic = new ClinicDetailResponse() { Id = id,};

        var viewModel = builder
            .SaveOnSession<ClinicDetailResponse>(SESSION.KEY_CLINIC_DETAIL_SELECTED, clinic)
            .Build();

        await viewModel.OnAppearingAsync();

        Assert.That(viewModel.NewClinicModel.Id, Is.EqualTo(id));
    }
}
