using System.Windows.Input;

namespace FreyaManager.Features.Subscriptions;

public class NewSubscriptionViewModel : CoreViewModel
{
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private readonly SubscriptionService subscriptionService;
    private int sizeSelectedIndex;

    public NewSubscriptionViewModel(
        INavigationService navigationService,
        IDialogService dialogService,
        SubscriptionService subscriptionService)
    {
        Title = "Nueva subscripción";
        ShowBackButton = true;
        NewSubscription = new NewSubscriptionValidtable();

        this.navigationService = navigationService;

        BackCommand = new AsyncCommand(BackCommandExecute);
        CreateSubscriptionCommand = new AsyncCommand(CreateSubscriptionCommandExecute);
        this.dialogService = dialogService;
        this.subscriptionService = subscriptionService;

    }

    public NewSubscriptionValidtable NewSubscription { get; set; }

    public ICommand CreateSubscriptionCommand { get; set; }

    public IAsyncCommand BackCommand { get; set; }

    public Dictionary<string, string> SizesAvailables { get; set; } = new Dictionary<string, string>()
    {
        {"10 MB", "10485760" },
        {"100 MB", "104857600" },
        {"250 MB", "262144000" },
        {"500 MB", "524288000" },
        {"750 MB", "786432000" },
        {"1 GB", "1073741824" },
        {"2 GB", "2147483648" },
    };

    public int SizeSelectedIndex
    {
        get { return sizeSelectedIndex; }
        set
        {
            SetAndRaisePropertyChanged(ref sizeSelectedIndex, value);
        }
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    private async Task CreateSubscriptionCommandExecute()
    {
        if (NewSubscription.HasError(out var error))
        {
            await dialogService.ShowMessage(
                "Error",
                error);

            return ;
        }

        if (SizeSelectedIndex < 0)
        {
            await dialogService.ShowMessage(
                "Error",
                "Selecciona un espacio");
            return;
        }

        var sizeSelected = SizesAvailables.ElementAt(SizeSelectedIndex);
        IsBusy = true;

        long? annualPrice = NewSubscription.HasAnnualPrice.Value ? long.Parse(NewSubscription.PriceAnnual.Value) : null;

        var created = await subscriptionService.CreateSubscriptionProduct(
            NewSubscription.Name.Value, 
            NewSubscription.Description.Value, 
            long.Parse(NewSubscription.PriceMonthly.Value),
            annualPrice,
            sizeSelected.Value,
            NewSubscription.AllowPromotionCodes.Value);

        IsBusy = false;
        if (created) 
        {
            await dialogService.ShowMessage(
                "Creada",
                "Se ha creado la subscripción con exito");
            await navigationService.BackAsync();
        }

        return ;
    }

}
