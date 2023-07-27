using System;
using System.Windows.Controls;

namespace Freya.Desktop.Dialogs.Interfaces;

public interface IDialogWindowsService
{
    Grid GetDialogContainer();

    void Close(Guid id);

    void CloseAll();
}
