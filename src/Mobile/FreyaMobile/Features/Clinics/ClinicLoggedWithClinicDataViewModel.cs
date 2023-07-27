namespace FreyaMobile.Features.Clinics;

public class ClinicLoggedWithClinicDataViewModel : CoreViewModel
{
    private readonly MenuClinicService menuClinicService;
    private IEnumerable<ClinicMenuItem> menuItems;
    private ClinicMenuItem? selectedItem;


    public ClinicLoggedWithClinicDataViewModel(
        MenuClinicService menuClinicService)
    {
        this.menuClinicService = menuClinicService;
        this.SelectionChangedCommand = new AsyncCommand(SelectionChangedAsync);

        menuItems = menuClinicService.GenerateMenu();
    }

    public IAsyncCommand SelectionChangedCommand { get; set; }

    public ClinicMenuItem? SelectedItem { get => selectedItem; set => SetAndRaisePropertyChanged(ref selectedItem, value); }

    public IEnumerable<ClinicMenuItem> MenuItems
    {
        get => menuItems;
        set => SetAndRaisePropertyChanged(ref menuItems, value);
    }

    private async Task SelectionChangedAsync()
    {
        if (selectedItem == null)
        {
            return;
        }

        var page = menuClinicService.GetPageFromMenuItemType(selectedItem.Type);

        SelectedItem = null;

        await Shell.Current.GoToAsync(page);
    }
}
