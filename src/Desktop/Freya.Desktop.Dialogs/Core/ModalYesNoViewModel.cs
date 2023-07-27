using Freya.Desktop.Dialogs.Interfaces;
using System;

namespace Freya.Desktop.Dialogs.Core;

internal class ModalYesNoViewModel
{
    private readonly IDialogWindowsService dialogWindowsService;

    public ModalYesNoViewModel(IDialogWindowsService dialogWindowsService)
    {
        this.dialogWindowsService = dialogWindowsService;
    }

    public void Close(Guid dialogId)
    {
        dialogWindowsService?.Close(dialogId);
    }
}
