using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

namespace FreyaPayments.Core;

public static class FreyaPaymentsCollection
{
    public static IServiceCollection UsePaymentsCore(this IServiceCollection serviceProvider, IConfiguration configuration)
    {
        serviceProvider.AddTransient<IProductsPaymentsService, ProductsPaymentsService>();
        serviceProvider.AddTransient<IPaymentSesionService, PaymentSesionService>();

        var key = configuration.GetRequiredSection("Stripe:Key").Value;
        StripeConfiguration.ApiKey = key; 

        return serviceProvider;
    }
}
