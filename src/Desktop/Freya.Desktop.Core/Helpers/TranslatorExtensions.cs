namespace Freya.Desktop.Core.Helpers;

public static class TranslatorExtensions
{
    public static string GetTextByRol(this ITranslatorService translatorService, EmployeeRol item)
    {
        var key = RolEmployeeTextHelper.GetRolResourceKeyByRol(item);
        return translatorService.Translate(key);
    }
}
