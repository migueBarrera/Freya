using Models.Core.AppManagers;

namespace FreyaManager.Services
{
    public interface ICurrentAppManagerService
    {
        public AppManager? Employee { get; }

        Task Set(AppManager? employee);
    }
}
