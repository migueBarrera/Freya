using Freya.Desktop.Dialogs.Interfaces;
using System;

namespace Freya.Desktop.Dialogs.Core
{
    internal class SimpleViewModel
    {
        private readonly IDialogWindowsService dialogWindowsService;

        public SimpleViewModel(IDialogWindowsService dialogWindowsService)
        {
            this.dialogWindowsService = dialogWindowsService;
        }

        public void Close(Guid dialogId)
        {
            dialogWindowsService?.Close(dialogId);
        }
    }
}
