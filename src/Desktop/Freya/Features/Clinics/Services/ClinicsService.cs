using Models.Core.Common;

namespace Freya.Features.Clinics.Services;

public class ClinicsService : IClinicsService
{
    private readonly IClinicAPIService clinicApiService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ICurrentEmployeeService currentEmployeeService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<ClinicsService> logger;

    public ClinicsService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        IDialogService dialogService,
        ICurrentEmployeeService currentEmployeeService,
        ILogger<ClinicsService> logger,
        ITranslatorService translatorService)
    {
        this.clinicApiService = refitService.InitRefitInstance<IClinicAPIService>(true);
        this.taskHelperFactory = taskHelperFactory;
        this.dialogService = dialogService;
        this.currentEmployeeService = currentEmployeeService;
        this.logger = logger;
        this.translatorService = translatorService;
    }

    public async Task<PagedModel<ClinicResponse>> GetClinicsAsync(PaginationFilter PaginationFilter)
    {
        var companyId = currentEmployeeService.Employee!.CompanyId;
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnGetClientsError).
                               TryExecuteAsync(
                               () => clinicApiService.GetClinicsByCompanyIdAsync(companyId, PaginationFilter));
        if (result.IsSuccess)
        {
            return result.Value;
        }

        return PagedModel<ClinicResponse>.Empty();
    }

    private async Task<bool> OnGetClientsError(Exception arg)
    {
        if (arg is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await dialogService.ShowMessage(translatorService.Translate("error"),
                string.Empty);
            return true;
        }

        return false;
    }
}
