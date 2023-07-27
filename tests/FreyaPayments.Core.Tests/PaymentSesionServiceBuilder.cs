using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Stripe;

namespace FreyaPayments.Core.Tests;

public class PaymentSesionServiceBuilder
{
    public PaymentSesionService Build()
    {
        StripeConfiguration.ApiKey = Consts.APIKEY;

        ILogger<PaymentSesionService> nullLogger = new NullLogger<PaymentSesionService>();
        return new PaymentSesionService(nullLogger);
    }
}
