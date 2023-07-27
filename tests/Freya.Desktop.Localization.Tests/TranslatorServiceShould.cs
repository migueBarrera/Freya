using System.Globalization;

namespace Freya.Desktop.Localization.Tests;

[TestFixture]
public class TranslatorServiceShould
{
    private TranslatorServiceBuilder builder;

    [SetUp]
    public void Setup()
    {
        builder = new TranslatorServiceBuilder();
    }

    [Test]
    [Ignore("De momento no funciona el multiidoma")]
    public void CanTranslateOnEnUS()
    {
        var service = builder.Build();

        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
        var value = service.Translate("enter");

        Assert.That(value, Is.EqualTo("Enter"));
    }

    [Test]
    public void CanTranslateOnEsES()
    {
        var service = builder.Build();

        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-ES");
        var value = service.Translate("enter");

        Assert.That(value, Is.EqualTo("Entrar"));
    }
    
    [Test]
    public void CanTranslateOnEs()
    {
        var service = builder.Build();

        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es");
        var value = service.Translate("enter");

        Assert.That(value, Is.EqualTo("Entrar"));
    }
}
