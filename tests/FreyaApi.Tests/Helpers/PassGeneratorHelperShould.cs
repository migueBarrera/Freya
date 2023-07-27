using FreyaApi.Helpers;

namespace FreyaApi.Tests.Helpers;

[TestFixture]
public class PassGeneratorHelperShould
{
    [Test]
    public void GenerateAPasswordNotEmpty()
    {
        var value = PassGeneratorHelper.Generate();

        Assert.That(string.IsNullOrWhiteSpace(value), Is.False);
    }

    [Test]
    public void NotGenerateTwoPasswordEquals()
    {
        var value = PassGeneratorHelper.Generate();
        var value1 = PassGeneratorHelper.Generate();
        var value2 = PassGeneratorHelper.Generate();
        var value3 = PassGeneratorHelper.Generate();
        var value4 = PassGeneratorHelper.Generate();
        var value5 = PassGeneratorHelper.Generate();
        Assert.Multiple(() =>
        {
            Assert.That(value, Is.Not.SameAs(value1));
            Assert.That(value1, Is.Not.SameAs(value2));
            Assert.That(value2, Is.Not.SameAs(value3));
            Assert.That(value4, Is.Not.SameAs(value5));
        });
    }
}
