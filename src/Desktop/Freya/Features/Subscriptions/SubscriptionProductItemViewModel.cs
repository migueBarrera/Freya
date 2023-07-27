using Freya.Desktop.Core.Converters;
using System.Windows.Input;

namespace Freya.Features.Subscriptions;

public class SubscriptionProductItemViewModel : ObservableObject, INavigationAwareViewModel
{
    private readonly ISessionService sessionService;
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterAnalitics analitics;

    public SubscriptionProductItemViewModel(
        ISessionService sessionService,
        INavigationService navigationService,
        IDialogService dialogService,
        ITranslatorService translatorService,
        AppCenterAnalitics analitics)
    {
        GoToCheckoutCommand = new AsyncCommand<PaymentsPlansForCompanyResponse>(GoToCheckoutCommandExecute);

        this.sessionService = sessionService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
        this.analitics = analitics;
    }

    public PaymentsPlansForCompanyResponse? Data { get; set; }

    public ICommand GoToCheckoutCommand { get; set; }

    public bool IsSubscriptionActive { get; set; }

    public bool ClinicHasASubcriptionActive { get; set; }

    public string Size { get; set; } = string.Empty;

    public Task OnAppearingAsync(object? parameter = null)
    {
        Data = (parameter as PaymentsPlansForCompanyResponse) ?? new PaymentsPlansForCompanyResponse();
        string sizeOnText = SizeToTextConverter.ConvertToText(Data.Size);
        var sizeTextUnformatted = translatorService.Translate("payment_product_info_row_money");
        Size = string.Format(sizeTextUnformatted, sizeOnText);

        return Task.CompletedTask;
    }

    public Task OnDisappearingAsync()
    {
        return Task.CompletedTask;
    }

    private async Task GoToCheckoutCommandExecute(PaymentsPlansForCompanyResponse arg)
    {
        analitics.CheckOut(arg.Name, arg.ProductId, arg.CompanyId, ClinicHasASubcriptionActive, mensual: true);
        analitics.Clicked("Go to checkout");
        if (ClinicHasASubcriptionActive) 
        {
            await dialogService.ShowMessage(
                translatorService.Translate("dialog_clinic_subscrption_want_cancel_but_has_other_active_title"),
                translatorService.Translate("dialog_clinic_subscrption_want_cancel_but_has_other_active_body"));
            return;
        }
        
        sessionService.Save(SESSION.KEY_SUBSCRIPTION_SELECTED, arg);
        await navigationService.NavigateTo(typeof(CheckoutPage));
    }
}
