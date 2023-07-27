using Models.Core.Common;
using Models.Core.Employees;

namespace FreyaManager.Features.Clinics.Models
{
    public class ClinicDetailEmployeesViewModel : CoreViewModel
    {
        private readonly EmployeeService employeeService;
        private readonly INavigationService navigationService;
        private readonly ISessionService sessionService;
        private PagedModel<EmployeeModel> employees;
        public PaginationFilter paginationFilter = new PaginationFilter();
        private Guid clinicId;

        public ClinicDetailEmployeesViewModel(
            EmployeeService employeeService,
            INavigationService navigationService,
            ISessionService sessionService)
        {
            employees = PagedModel<EmployeeModel>.Empty();
            LessItemsCommand = new AsyncCommand(LessItemsCommandExecute);
            MoreItemsCommand = new AsyncCommand(MoreItemsCommandExecute);
            this.employeeService = employeeService;
            this.navigationService = navigationService;
            this.sessionService = sessionService;
        }

        public IAsyncCommand LessItemsCommand { get; set; }

        public IAsyncCommand MoreItemsCommand { get; set; }

        public PagedModel<EmployeeModel> Employees { get => employees; set => SetAndRaisePropertyChanged(ref employees, value); }

        internal Task LoadEmployees(PagedModel<EmployeeResponse>? employees, Guid clinicId)
        {
            this.clinicId = clinicId;
            Employees = ConvertToEmployeeItemViewModel(employees ?? PagedModel<EmployeeResponse>.Empty());
            paginationFilter = new PaginationFilter(Employees.CurrentPage, Employees.PageSize);

            return Task.CompletedTask;
        }

        private PagedModel<EmployeeModel> ConvertToEmployeeItemViewModel(PagedModel<EmployeeResponse> items)
        {
            var viewmodels = PagedModel<EmployeeModel>.EmptyFrom<EmployeeResponse>(items);

            foreach (var item in items.Items)
            {
                viewmodels.Items.Add(new EmployeeModel(item, navigationService, sessionService));
            }

            return viewmodels;
        }

        private async Task MoreItemsCommandExecute()
        {
            paginationFilter.PageNumber++;
            await GetEmployeesAsync();
        }

        private async Task LessItemsCommandExecute()
        {
            paginationFilter.PageNumber--;
            await GetEmployeesAsync();
        }

        private async Task GetEmployeesAsync()
        {
            Parent!.IsBusy = true;
            var employees = await employeeService.GetEmployeesAsync(clinicId, paginationFilter);
            Employees = ConvertToEmployeeItemViewModel(employees);
            Parent!.IsBusy = false;
        }
    }
}
