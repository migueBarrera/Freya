namespace Freya.Tests.Features.Clinics;

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
    public void CanBuild_EditClinic()
    {
        var viewModel = builder.Build();
    }
}
