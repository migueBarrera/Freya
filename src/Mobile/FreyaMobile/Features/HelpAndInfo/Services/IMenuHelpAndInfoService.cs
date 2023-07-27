using FreyaMobile.Features.HelpAndInfo.Models;

namespace FreyaMobile.Features.HelpAndInfo.Services;

public interface IMenuHelpAndInfoService
{
    IEnumerable<HAIMenuItem> GenerateMenu();

    string GetPageFromMenuItemType(TypeHAIMenu type);
}
