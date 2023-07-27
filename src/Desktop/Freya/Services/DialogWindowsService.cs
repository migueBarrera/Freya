using System.Windows.Controls;

namespace Freya.Services;

public class DialogWindowsService : IDialogWindowsService
{
    public void Close(Guid id)
    {
        UIElement? removableItem = GetDialogItem(id);

        if (removableItem != null)
        {
            GetDialogContainer().Children.Remove(removableItem);
        }
    }

    public void CloseAll()
    {
        GetDialogContainer().Children.Clear();
    }

    public Grid GetDialogContainer()
    {
        return ((MainWindow)App.Current.MainWindow).DialogContainer;
    }

    private UIElement? GetDialogItem(Guid id)
    {
        UIElement? removableItem = null;
        var dialogs = GetDialogContainer().Children;

        foreach (var item in dialogs)
        {
            if (item is CoreModalDialog)
            {
                var dialog = item as CoreModalDialog;
                if (dialog?.DialogId == id)
                {
                    removableItem = item as UIElement;
                    break;
                }
            }
        }

        return removableItem;
    }
}
