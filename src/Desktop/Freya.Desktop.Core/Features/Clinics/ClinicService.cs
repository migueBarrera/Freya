using ApiContract.Interfaces;
using Models.Core.Clinics;
using Models.Core.Common;
using Refit;

namespace Freya.Desktop.Core.Features.Clinics;

public class ClinicService : IClinicService
{
    private readonly IClinicAPIService clinicAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<ClinicService> logger;

    public ClinicService(
        ITaskHelperFactory taskHelperFactory,
        IRefitService refitService,
        ILogger<ClinicService> logger,
        ITranslatorService translatorService,
        IDialogService dialogService)
    {
        this.taskHelperFactory = taskHelperFactory;
        clinicAPIService = refitService.InitRefitInstance<IClinicAPIService>(isAutenticated: true);
        this.logger = logger;
        this.translatorService = translatorService;
        this.dialogService = dialogService;
    }

    public async Task<bool> CreateClinic(ClinicCreateRequest clinic)
    {
        var result = await taskHelperFactory.
                                CreateInternetAccessViewModelInstance(logger).
                                TryExecuteAsync(
                                () => clinicAPIService.CreateAsync(clinic));

        return result.IsSuccess;
    }

    public async Task<ClinicDetailResponse> GetClinic(Guid id)
    {
        var result = await taskHelperFactory.
                                CreateInternetAccessViewModelInstance(logger).
                                TryExecuteAsync(
                                () => clinicAPIService.GetAsync(id));

        if (result.IsSuccess)
        {
            return result.Value;
        }

        return new ClinicDetailResponse();
    }

    public async Task<PagedModel<ClinicResponse>> GetClinicsByCompanyId(Guid id, PaginationFilter PaginationFilter)
    {
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnGetClientsError).
                               TryExecuteAsync(
                               () => clinicAPIService.GetClinicsByCompanyIdAsync(id, PaginationFilter));
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

    public async Task<bool> DeleteClinic(Guid id)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => clinicAPIService.Delete(id));

        return result.IsSuccess;
    }
}
