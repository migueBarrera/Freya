using Models.Core.AppManagers;

namespace FreyaManager.Services
{
    public class CurrentAppManagerService : ICurrentAppManagerService
    {
        private AppManager? _employee;
        public AppManager? Employee => _employee;

        public Task Set(AppManager? employee)
        {
            _employee = employee;
            return Task.CompletedTask;
        }
    }
}
