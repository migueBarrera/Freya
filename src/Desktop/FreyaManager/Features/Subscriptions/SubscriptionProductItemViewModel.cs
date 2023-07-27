using Freya.Desktop.Core.Helpers;
using Models.Core.Subscriptions;

namespace FreyaManager.Features.Subscriptions;

public class SubscriptionProductItemViewModel : CoreViewModel
{
    private readonly IConfiguration configuration;

    public SubscriptionProductItemViewModel(IConfiguration configuration)
    {
        SeeProductOnStripeCommand = new AsyncCommand(SeeProductOnStripeCommandExecute);
        this.configuration = configuration;
    }

    public SubscriptionProductResponse? Data { get; set; }

    public string Size { get; set; } = string.Empty;

    public IAsyncCommand SeeProductOnStripeCommand { get; set; }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        Data = (parameter as SubscriptionProductResponse) ?? new SubscriptionProductResponse();
        var sizeOnGB = SizeHelper.ConvertBytesToMB(Data.Size);
        Size = sizeOnGB < 1L ? $"Disponde de {sizeOnGB} MB por cada cliente" : $"Disponde de {sizeOnGB} MB por cada cliente"; // TODO mejorar
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
