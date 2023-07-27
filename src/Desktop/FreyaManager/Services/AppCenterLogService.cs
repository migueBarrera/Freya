using FreyaManager.Framework;

namespace FreyaManager.Services
{
    internal class AppCenterLogService : IAppCenterLogService
    {
        public string LogFolderPath => LogPathHelper.LogFolderPath;
    }
}
