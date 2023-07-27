using Models.Core.Common;

namespace Freya.Features.Clinics.Models;

public class ClinicDetailEmployeesViewModel : CoreViewModel
{
    private readonly IServiceProvider serviceProvider;
    private readonly IEmployeeService employeeService;
    private PagedModel<EmployeeItemViewModel> employees;
    public PaginationFilter paginationFilter = new PaginationFilter();

    public ClinicDetailEmployeesViewModel(IServiceProvider serviceProvider, IEmployeeService employeeService)
    {
        this.serviceProvider = serviceProvider;

        employees = PagedModel<EmployeeItemViewModel>.Empty();
        LessItemsCommand = new AsyncCommand(LessItemsCommandExecute);
        MoreItemsCommand = new AsyncCommand(MoreItemsCommandExecute);
        this.employeeService = employeeService;
    }

    public IAsyncCommand LessItemsCommand { get; set; }

    public IAsyncCommand MoreItemsCommand { get; set; }

    public PagedModel<EmployeeItemViewModel> Employees { get => employees; set => SetAndRaisePropertyChanged(ref employees, value); }

    internal async Task LoadEmployees(PagedModel<EmployeeResponse>? employees)
    {
        Employees = await ConvertToEmployeeItemViewModel(employees ?? PagedModel<EmployeeResponse>.Empty());
        paginationFilter = new PaginationFilter(Employees.CurrentPage, Employees.PageSize);
    }

    private async Task<PagedModel<EmployeeItemViewModel>> ConvertToEmployeeItemViewModel(PagedModel<EmployeeResponse> items)
    {
        var viewmodels = PagedModel<EmployeeItemViewModel>.EmptyFrom<EmployeeResponse>(items);

        foreach (var item in items.Items)
        {
            if (serviceProvider.GetService(typeof(EmployeeItemViewModel)) is EmployeeItemViewModel vm)
            {
                await vm.OnAppearingAsync(item);
                vm.ClinicDetailViewModel = this;
                viewmodels.Items.Add(vm);
            }
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
        var employees = await employeeService.GetEmployeesAsync(paginationFilter);
        Employees = await ConvertToEmployeeItemViewModel(employees);
        Parent!.IsBusy = false;
    }
}
