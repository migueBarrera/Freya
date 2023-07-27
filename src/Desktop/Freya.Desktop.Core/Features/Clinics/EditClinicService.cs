using ApiContract.Interfaces;
using Models.Core.Clinics;

namespace Freya.Desktop.Core.Features.Clinics;

public class EditClinicService : IEditClinicService
{
    private readonly IClinicAPIService clinicAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<EditClinicService> logger;

    public EditClinicService(
        ITaskHelperFactory taskHelperFactory,
        IRefitService refitService,
        ILogger<EditClinicService> logger,
        IDialogService dialogService,
        ITranslatorService translatorService)
    {
        this.taskHelperFactory = taskHelperFactory;
        clinicAPIService = refitService.InitRefitInstance<IClinicAPIService>(isAutenticated: true);
        this.logger = logger;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
    }

    public async Task<bool> EditClinic(UpdateClinicRequest clinic)
    {
        var result = await taskHelperFactory.
                                CreateInternetAccessViewModelInstance(logger).
                                WithErrorHandling(ErrorHandlerCreateClinic).
                                TryExecuteAsync(
                                () => clinicAPIService.Update(clinic));

        return result.IsSuccess;
    }

    private async Task<bool> ErrorHandlerCreateClinic(Exception arg)
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
