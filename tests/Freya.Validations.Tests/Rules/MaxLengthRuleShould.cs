using Freya.Validations.Rules;

namespace Freya.Validations.Tests.Rules;

[TestFixture]
public class MaxLengthRuleShould
{
    [Test]
    [TestCase(1, "")]
    [TestCase(10, "123456789")]
    [TestCase(11, "0123456789")]
    public void Should_return_max_length(int maxLengthForRule, string textForCheck)
    {
        var rule = new MaxLengthRule("test message", maxLengthForRule);

        Assert.That(rule.Check(textForCheck), Is.True);
    }
    
    [Test]
    [TestCase(1, "11")]
    [TestCase(10, "0123456789")]
    [TestCase(11, "01234567891")]
    public void Should_return_max_length_failed(int maxLengthForRule, string textForCheck)
    {
        var rule = new MaxLengthRule("test message", maxLengthForRule);

        Assert.That(rule.Check(textForCheck), Is.False);
    }
}
