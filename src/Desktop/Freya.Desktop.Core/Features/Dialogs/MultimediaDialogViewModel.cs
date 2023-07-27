using MvvmHelpers.Commands;

namespace Freya.Desktop.Core.Features.Dialogs;

public class MultimediaDialogViewModel
{
    private readonly IDialogService dialogService;
    private readonly AppCenterAnalitics appCenterAnalitics;

    public AsyncCommand CancelCommand { get; set; }

    public MultimediaDialogViewModel(
        IDialogService dialogService,
        AppCenterAnalitics appCenterAnalitics)
    {
        CancelCommand = new AsyncCommand(CancelCommandExecute);
        this.dialogService = dialogService;
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(MultimediaDialogViewModel));
    }

    private Task CancelCommandExecute()
    {
        dialogService.CloseAll();
        return Task.CompletedTask;
    }
}
