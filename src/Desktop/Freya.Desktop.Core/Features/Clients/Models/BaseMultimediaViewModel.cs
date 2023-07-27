using Freya.Desktop.Core.Features.Clients.Services;
using Freya.Desktop.Core.Features.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Models.Core.Multimedia;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using System.IO;

namespace Freya.Desktop.Core.Features.Clients.Models;

public abstract class BaseMultimediaViewModel : CoreViewModel
{
    private readonly IDialogService dialogService;
    private readonly MultimediaService multimediaService;
    private readonly ILogger<BaseMultimediaViewModel> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterService appCenterService;

    public BaseMultimediaViewModel(
        IDialogService dialogService,
        MultimediaService multimediaService,
        ILogger<BaseMultimediaViewModel> logger,
        IServiceProvider serviceProvider,
        ITranslatorService translatorService,
        AppCenterService appCenterService)
    {
        this.dialogService = dialogService;
        DeleteCommand = new AsyncCommand(DeleteCommandExecute);
        DownloadCommand = new AsyncCommand(DownloadCommandExecute);
        SeeCommand = new AsyncCommand(SeeCommandExecute);
        this.multimediaService = multimediaService;
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.translatorService = translatorService;
        this.appCenterService = appCenterService;
    }

    public new IMultimediaItemRemoveToParentModel? Parent { get; set; }

    public abstract MultimediaType MultimediaType { get; }

    public IAsyncCommand DeleteCommand { get; set; }

    public IAsyncCommand DownloadCommand { get; set; }

    public IAsyncCommand SeeCommand { get; set; }

    private async Task DeleteCommandExecute()
    {
        await dialogService.ShowMessageYesNo(
                translatorService.Translate("dialog_multimedia_remove_title"),
                translatorService.Translate("dialog_multimedia_remove_body"),
                new AsyncCommand(async () =>
                {
                    IsBusy = true;
                    var deleted = await multimediaService.Delete(GetMultimediaId());
                    IsBusy = false;
                    if (deleted)
                    {
                        Parent?.RemoveItem(GetMultimediaId());
                    }
                }));
    }

    internal abstract Guid GetMultimediaId();

    internal abstract Uri GetMultimediaUri();

    private async Task DownloadCommandExecute()
    {
        // Prepare a dummy string, thos would appear in the dialog
        string dummyFileName = "Save Here";

        SaveFileDialog sf = new SaveFileDialog();
        // Feed the dummy name to the save dialog
        sf.FileName = dummyFileName;
        sf.Filter = "Directory | directory";
        sf.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        if (sf.ShowDialog() == true)
        {
            var multimediaURi = GetMultimediaUri();
            var fileName = Path.GetFileName(multimediaURi.ToString());
            string? savePath = Path.GetDirectoryName(sf.FileName);
            var newfileName = Path.Combine(savePath!, fileName);

            var isDownloaded = await GetAndDownload(GetMultimediaUri(), newfileName);
            await dialogService.ShowMessage(
                  translatorService.Translate("dialog_multimedia_download_file_title"),
                  isDownloaded
                      ? translatorService.Translate("dialog_multimedia_download_file_success_body")
                      : translatorService.Translate("dialog_multimedia_download_file_fail_body"));
        }
    }

    public async Task<bool> GetAndDownload(Uri uri, string newFileName)
    {
        bool result = false;

        try
        {
            var client = new HttpClient();
            byte[] imageBytes = await client.GetByteArrayAsync(uri);
            if (imageBytes != null)
            {
                using var fs = new FileStream(newFileName, FileMode.Create);
                fs.Write(imageBytes, 0, imageBytes.Length);
                result = true;
            }
        }
        catch (Exception e)
        {
            appCenterService.TrackError(e);
            logger.LogError(e.Message, e);
        }

        return result;
    }

    private async Task SeeCommandExecute()
    {
        var viewModel = serviceProvider.GetService<MultimediaDialogViewModel>();
        var uri = GetMultimediaUri();
        switch (MultimediaType)
        {
            case MultimediaType.ULTRASOUND:
                var view = new ImagePlayerDialog(uri)
                {
                    DataContext = viewModel,
                };
                await dialogService.ShowDialog(view);
                break;
            case MultimediaType.VIDEO:
                var viewVideo = new ViewPlayerDialog(uri)
                {
                    DataContext = viewModel,
                };
                await dialogService.ShowDialog(viewVideo);
                break;
            case MultimediaType.SOUND:
                var viewSound = new SoundPlayerDialog(uri)
                {
                    DataContext = viewModel,
                };
                await dialogService.ShowDialog(viewSound);
                break;
            default:
                break;
        }

    }
}
