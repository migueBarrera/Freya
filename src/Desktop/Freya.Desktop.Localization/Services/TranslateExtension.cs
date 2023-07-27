namespace Freya.Desktop.Localization.Services;

public class TranslateExtension : MarkupExtension
{
    private string key;

    public TranslateExtension(string key)
    {
        this.key = key;
    }

    [ConstructorArgument("key")]
    public string Key
    {
        get
        {
            return key;
        }

        set
        {
            key = value;
        }
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return TranslatorHelper.Translate(Key);
    }
}
