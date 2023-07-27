using Freya.Desktop.Core.Helpers;

namespace Freya.Desktop.Core.Tests.Helpers;

[TestFixture]
public class SizeHelperShould
{
    [TestCase(0L,0L)]
    [TestCase(1048576L, 1L)]
    [TestCase(10485760L, 10L)]
    [TestCase(1073741824L, 1024L)]
    public void ConvertCorrectlyBytesToMB(long bytes, long megaBytesExpected)
    {
        var mb = SizeHelper.ConvertBytesToMB(bytes);

        Assert.That(mb, Is.EqualTo(megaBytesExpected));
    }
}
