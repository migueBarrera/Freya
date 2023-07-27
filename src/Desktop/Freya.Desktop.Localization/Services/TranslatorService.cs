namespace Freya.Desktop.Localization.Services;

public class TranslatorService : ITranslatorService
{
    public string Translate(string toTranslate)
    {
        return TranslatorHelper.Translate(toTranslate);
    }
}
