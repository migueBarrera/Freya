using Microsoft.Extensions.Logging.Abstractions;
using Stripe;

namespace FreyaPayments.Core.Tests;

public class ProductsPaymentsServiceBuilder
{
    public ProductsPaymentsService Build()
    {
        StripeConfiguration.ApiKey = Consts.APIKEY;
        var logger = new NullLogger<ProductsPaymentsService>();
        return new ProductsPaymentsService(logger);
    }
}
