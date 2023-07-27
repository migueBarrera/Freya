namespace Freya.Features.Clients.Services;

internal class MultimediaFilePickerService : IMultimediaFilePickerService
{
    private IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private ISessionManagerService sessionManagerService;
    private readonly ILogger<MultimediaFilePickerService> logger;
    private readonly IServiceProvider serviceProvider;

    public MultimediaFilePickerService(
        ILogger<MultimediaFilePickerService> logger,
        ISessionManagerService sessionManagerService,
        ITranslatorService translatorService,
        IDialogService dialogService,
        IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.sessionManagerService = sessionManagerService;
        this.translatorService = translatorService;
        this.dialogService = dialogService;
        this.serviceProvider = serviceProvider;
    }

    public async Task<bool> OpenFilePickerAndUploadMultimedia(
            string filter,
            MultimediaType multimediaType,
            Guid clientId,
            Guid clinicId,
            string clinicName,
            string clientName,
            long limitSize,
            long spaceUsedByClient)
    {
        logger.LogInformation("Open file picker for upload multimedia items");
        var fileDialog = new OpenFileDialog
        {
            Filter = filter,
            Multiselect = true,
            CheckPathExists = true,
        };

        if (fileDialog.ShowDialog() != true)
        {
            logger.LogInformation("Discarting because ShowDialog false");
            return false;
        }

        var totalSize = fileDialog.FileNames.Select(path => new FileInfo(path)).Sum(x => x.Length);

        logger.LogInformation("Check space for selected items : {0}", totalSize);
        var spaceForUploadingItemsOnStack = 0L;
        if (sessionManagerService.GetCurrentsSessions().Where(x => x.IsInProgress && x.ClientId == clientId).Any())
        {
            spaceForUploadingItemsOnStack = sessionManagerService.GetCurrentsSessions()
            .Where(x => x.IsInProgress && x.ClientId == clientId)
            .Sum(x => x.Size);
            logger.LogInformation("Check space for uploading progress items : {0}", spaceForUploadingItemsOnStack);
        }

        var maxSpaceSvailable = limitSize - spaceUsedByClient;
        maxSpaceSvailable = maxSpaceSvailable - spaceForUploadingItemsOnStack;

        logger.LogInformation("Max space avalable for this client : {0}", maxSpaceSvailable);
        if (totalSize > maxSpaceSvailable)
        {
            logger.LogInformation("Discarting upload because this client han not space available");
            await dialogService.ShowMessage(
                translatorService.Translate("dialog_error_client_space_available_limited_title"),
                translatorService.Translate("dialog_error_client_space_available_limited_body"));
            return false;
        }

        logger.LogInformation("Continue upload items beacuuse this client has space avalable");

        foreach (var item in fileDialog.FileNames)
        {
            var newSession = serviceProvider.GetRequiredService<SessionViewModel>();
            newSession.Path = item;
            newSession.ClientId = clientId;
            newSession.ClinicId = clinicId;
            newSession.MultimediaType = multimediaType;
            newSession.ClientName = clientName;
            newSession.ClinicName = clinicName;

            await sessionManagerService.AddNewMultimedia(newSession);
        }

        return true;
    }
}
