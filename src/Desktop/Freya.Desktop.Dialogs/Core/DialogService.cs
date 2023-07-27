using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Freya.Desktop.Dialogs.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Freya.Desktop.Dialogs.Core;

internal class DialogService : IDialogService
{
    private readonly IDialogWindowsService dialogWindowsService;
    private readonly Application app;
    private readonly IServiceProvider serviceProvider;

    public DialogService(
        IDialogWindowsService dialogWindowsService,
        Application app,
        IServiceProvider serviceProvider)
    {
        this.dialogWindowsService = dialogWindowsService;
        this.app = app;
        this.serviceProvider = serviceProvider;
    }

    public void CloseAll()
    {
        app.Dispatcher.Invoke(delegate
        {
            dialogWindowsService.CloseAll();
        });
    }

    public Task ShowDialog(CoreModalDialog dialog)
    {
        app.Dispatcher.Invoke(delegate
        {
            dialogWindowsService.GetDialogContainer().Children.Add(dialog);
        });

        return Task.CompletedTask;
    }

    public Task ShowDialog<Dialog, ViewModel>() 
        where Dialog : CoreModalDialog
    {
        var dialog = serviceProvider.GetService<Dialog>();
        if (dialog == null)
        {
            return Task.CompletedTask;
        }
        var viewmodel = serviceProvider.GetService<ViewModel>();

        dialog.DataContext = viewmodel;
        app.Dispatcher.Invoke(delegate
        {
            dialogWindowsService.GetDialogContainer().Children.Add(dialog);
        });

        return Task.CompletedTask;
    }

    public Task ShowMessage(
        string title,
        string content)
    {
        var dialog = serviceProvider.GetService<SimpleDialog>();
        if (dialog == null)
        {
            return Task.CompletedTask;
        }
        var viewmodel = serviceProvider.GetService<SimpleViewModel>();

        dialog.DataContext = viewmodel;

        dialog.Title = title;
        dialog.SubTitle = content;

        app.Dispatcher.Invoke(delegate
        {
            dialogWindowsService.GetDialogContainer().Children.Add(dialog);
        });

        return Task.CompletedTask;
    }

    public Task ShowMessageYesNo(
        string title,
        string content,
        ICommand? yesCommand = null,
        ICommand? noCommand = null)
    {
        var dialog = serviceProvider.GetService<ModalYesNoDialog>();
        if (dialog == null)
        {
            return Task.CompletedTask;
        }

        var viewmodel = serviceProvider.GetService<ModalYesNoViewModel>();

        dialog.DataContext = viewmodel;
        dialog.Title = title;
        dialog.SubTitle = content;
        dialog.YesCommand = yesCommand;
        dialog.NoCommand = noCommand;

        app.Dispatcher.Invoke(delegate
        {
            dialogWindowsService.GetDialogContainer().Children.Add(dialog);
        });

        return Task.CompletedTask;
    }
}
