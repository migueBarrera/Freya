using FreyaMobile.Core.Framework;
using FreyaMobile.Features.HelpAndInfo.Models;
using FreyaMobile.Features.HelpAndInfo.Services;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;

namespace FreyaMobile.Features.HelpAndInfo;

public class HelpAndInfoViewModel : CoreViewModel
{
    private readonly IMenuHelpAndInfoService menuHelpAndInfoService;
    private IEnumerable<HAIMenuItem> menuItems;
    private HAIMenuItem selectedItem;

    public HelpAndInfoViewModel(
        IMenuHelpAndInfoService menuHelpAndInfoService)
    {
        this.menuHelpAndInfoService = menuHelpAndInfoService;
        this.SelectionChangedCommand = new AsyncCommand(SelectionChangedAsync);
        menuItems = Enumerable.Empty<HAIMenuItem>();
    }

    public IAsyncCommand SelectionChangedCommand { get; set; }

    public IEnumerable<HAIMenuItem> MenuItems
    {
        get => menuItems;
        set => SetAndRaisePropertyChanged(ref menuItems, value);
    }

    public HAIMenuItem SelectedItem
    {
        get => selectedItem;
        set => SetAndRaisePropertyChanged(ref selectedItem, value);
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        MenuItems = menuHelpAndInfoService.GenerateMenu();
    }

    private async Task SelectionChangedAsync()
    {
        if (selectedItem == null)
        {
            return;
        }

        var page = menuHelpAndInfoService.GetPageFromMenuItemType(selectedItem.Type);

        SelectedItem = null;

        await Shell.Current.GoToAsync(page);
        //await App.Current.MainPage.Navigation.PushAsync(page);
    }
}
