using Freya.Desktop.Dialogs.Core;
using Freya.Desktop.Dialogs.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Freya.Desktop.Dialogs;

public static class ServicesCollection
{
    public static void UseDesktopCoreDialogs<DialogWindowsServiceImplementation>(this IServiceCollection serviceProvider)
        where DialogWindowsServiceImplementation : class, IDialogWindowsService
    {
        serviceProvider.AddTransient<SimpleViewModel>();
        serviceProvider.AddTransient<ModalYesNoViewModel>();
        serviceProvider.AddTransient<SimpleDialog>();
        serviceProvider.AddTransient<ModalYesNoDialog>();
        serviceProvider.AddTransient<IDialogService, DialogService>();
        serviceProvider.AddTransient<IDialogWindowsService, DialogWindowsServiceImplementation>();
    }
}
