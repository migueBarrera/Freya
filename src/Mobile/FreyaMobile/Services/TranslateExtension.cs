namespace FreyaMobile.Services;

public class TranslateExtension : IMarkupExtension
{
    public string Key { get; set; } = string.Empty;

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return TranslatorHelper.Translate(Key);
    }
}
