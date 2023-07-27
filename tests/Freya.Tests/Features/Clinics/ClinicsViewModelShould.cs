namespace Freya.Tests.Features.Clinics;

[TestFixture]
public class ClinicsViewModelShould
{
    private ClinicsViewModelBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new ClinicsViewModelBuilder();
    }

    [Test]
    public void CanBuild_ClinicsViewModel()
    {
        var viewModel = builder.Build();
    }
}
