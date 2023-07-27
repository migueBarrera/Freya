using FreyaMobile.Resources.Localizations;
using System.Globalization;

namespace FreyaMobile.Services;

internal static class TranslatorHelper
{
    internal static string Translate(string toTranslate)
    {
        string translated = string.Empty;
        var culture = CultureInfo.CurrentCulture;
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
