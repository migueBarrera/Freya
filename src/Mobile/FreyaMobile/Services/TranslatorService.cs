namespace FreyaMobile.Services;

public class TranslatorService : ITranslatorService
{
    public string Translate(string toTranslate)
    {
        return TranslatorHelper.Translate(toTranslate);
    }
}
