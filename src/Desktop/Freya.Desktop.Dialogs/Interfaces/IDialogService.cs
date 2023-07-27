using System.Threading.Tasks;
using System.Windows.Input;
using Freya.Desktop.Dialogs.Core;

namespace Freya.Desktop.Dialogs.Interfaces;

/// <summary>
/// TODO Revisar si es mejor enviar el key para textos en lugar de tener la referencia en todos los proyectos de localization
/// </summary>
public interface IDialogService
{
    Task ShowMessage(
        string title,
        string content);

    Task ShowDialog(
        CoreModalDialog modalDialog);
    
    Task ShowDialog<Dialog, ViewModel>() where Dialog : CoreModalDialog;

    Task ShowMessageYesNo(
        string title,
        string content,
        ICommand? YesCommand = null,
        ICommand? NoCommand = null);

    void CloseAll();
}