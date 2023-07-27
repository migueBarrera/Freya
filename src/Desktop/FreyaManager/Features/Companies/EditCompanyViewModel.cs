using Models.Core.Companies;
using System.Windows.Input;

namespace FreyaManager.Features.Companies;

public class EditCompanyViewModel : CoreViewModel
{
    private readonly ISessionService sessionService;
    private readonly CompaniesService companiesService;
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private Guid _companyId = Guid.Empty;

    public EditCompanyViewModel(
        ISessionService sessionService,
        CompaniesService companiesService,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        Title = "Edit compañia";
        ShowBackButton = true;
        this.sessionService = sessionService;

        UpdateCompany = new UpdateCompanyValitable();
        this.companiesService = companiesService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;

        BackCommand = new AsyncCommand(BackCommandExecute);
        SaveChanges = new AsyncCommand(UpdateCompanyCommandExecute);
    }

    public UpdateCompanyValitable UpdateCompany { get; set; }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        var company = sessionService.Get<Company>(SESSION.KEY_COMPANY_SELECTED);

        _companyId = company.Id;
        UpdateCompany.Name.Value = company.Name;
        return base.OnAppearingAsync(parameter);
    }

    public ICommand SaveChanges { get; set; }

    public IAsyncCommand BackCommand { get; set; }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    private async Task UpdateCompanyCommandExecute()
    {
        if (UpdateCompany.HasError(out var error))
        {
            await dialogService.ShowMessage(
                "Error",
                error);

            return;
        }

        IsBusy = true;

        var updated = await companiesService.UpdateCompany(_companyId, UpdateCompany.Name.Value);

        IsBusy = false;
        if (updated)
        {
            await dialogService.ShowMessage(
                "Datos actualizados",
                "Se han actualizado los datos de la compañia con exito");
            await navigationService.BackAsync();
        }

        return;
    }
}
