using System.Threading;

namespace Freya.Desktop.Localization.Services;

public static class TranslatorHelper
{
    public static string Translate(string toTranslate)
    {
        string translated = string.Empty;
        //var culture = CultureInfo.DefaultThreadCurrentUICulture ?? new CultureInfo("es");
        var culture = new CultureInfo("es");
        try
        {
            translated = ResourceStrings.ResourceManager.GetString(toTranslate, culture) ?? string.Empty;
        }
        catch
        {
        }

        if (string.IsNullOrEmpty(translated))
        {
            translated = "NO translated resource found for: " + toTranslate + " in culture " + culture.Name;
        }

        return translated;
    }
}
