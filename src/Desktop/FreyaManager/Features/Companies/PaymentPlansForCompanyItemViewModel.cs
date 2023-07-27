using Freya.Desktop.Core.Helpers;
using Models.Core.Subscriptions;

namespace FreyaManager.Features.Companies;

public class PaymentPlansForCompanyItemViewModel : ObservableObject, INavigationAwareViewModel
{
    private readonly IConfiguration configuration;

    public PaymentPlansForCompanyItemViewModel(IConfiguration configuration)
    {
        SeeProductOnStripeCommand = new AsyncCommand(SeeProductOnStripeCommandExecute);
        this.configuration = configuration;
    }

    public string Size { get; set; } = string.Empty;

    public PaymentsPlansForCompanyResponse? Data { get; set; }

    public IAsyncCommand SeeProductOnStripeCommand { get; set; }

    public Task OnAppearingAsync(object? parameter = null)
    {
        Data = (parameter as PaymentsPlansForCompanyResponse) ?? new PaymentsPlansForCompanyResponse();
        var sizeOnGB = SizeHelper.ConvertBytesToMB(Data.Size);
        Size = sizeOnGB < 1L ? $"Disponde de {sizeOnGB} MB por cada cliente" : $"Disponde de {sizeOnGB} MB por cada cliente"; 
        return Task.CompletedTask;
    }

    public Task OnDisappearingAsync()
    {
        return Task.CompletedTask;
    }

    private Task SeeProductOnStripeCommandExecute()
    {
        bool isProductionEvironment = configuration.GetValue<bool>("IsProductionEvironment");
        string target = "https://dashboard.stripe.com/test/products/";
        if (isProductionEvironment)
        {
            target = "https://dashboard.stripe.com/products/";
        }

        var subs = Data?.ProductId;

        var sInfo = new System.Diagnostics.ProcessStartInfo(target + subs)
        {
            UseShellExecute = true,
        };
        System.Diagnostics.Process.Start(sInfo);

        return Task.CompletedTask;
    }
}
