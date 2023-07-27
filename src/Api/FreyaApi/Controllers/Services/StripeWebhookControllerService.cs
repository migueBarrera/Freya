
namespace FreyaApi.Controllers.Services;

public class StripeWebhookControllerService
{
    private ILogger<StripeWebhookControllerService> logger;
    private readonly IPaymentSesionService paymentSesionService;
    private readonly ISubscriptionPaymentRepository subscriptionPaymentRepository;
    private readonly ISubscriptionChargeRepository subscriptionChargeRepository;

    public StripeWebhookControllerService(
        ILogger<StripeWebhookControllerService> logger,
        IPaymentSesionService paymentSesionService,
        ISubscriptionPaymentRepository subscriptionPaymentRepository,
        ISubscriptionChargeRepository subscriptionChargeRepository)
    {
        this.logger = logger;
        this.paymentSesionService = paymentSesionService;
        this.subscriptionPaymentRepository = subscriptionPaymentRepository;
        this.subscriptionChargeRepository = subscriptionChargeRepository;
    }

    const string endpointSecret = "whsec_mbFfBTJVyHcLVpFkO3rl2htNRrOCef0T";
    const string endpointSecret2 = "whsec_CBFIuVWn5GQ8ZfiZHBbsejcdaUG7HgO6";

    public async Task<IActionResult> ProcessStripeWebhooks(ControllerBase controler)
    {
        try
        {
            var json = await new StreamReader(controler.HttpContext.Request.Body).ReadToEndAsync();

            Event? stripeEvent = GetStripeEvent(
                        controler.Request.Headers["Stripe-Signature"]!, 
                        json) 
                ?? throw new ArgumentException("Can not validate stripe signature");

            switch (stripeEvent.Type)
            {
                case Events.InvoicePaid:
                    {
                        return await HandleInvoicePayment(controler, (Invoice)stripeEvent.Data.Object);
                    }
                case Events.InvoicePaymentFailed:
                    {
                        return await HandleInvoicePaymentFailed(controler, (Invoice)stripeEvent.Data.Object);
                    }

                default:
                    logger.LogWarning("Unhandled event type: {0}", stripeEvent.Type);
                    break;
            }

            return controler.Ok();
        }
        catch (StripeException e)
        {
            logger.LogError(e, e.Message);
            return controler.BadRequest();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return controler.BadRequest();
        }
    }

    private Event? GetStripeEvent(string stripeSignature, string json)
    {
        Event? stripeEvent = null;
        try
        {
             stripeEvent = EventUtility.ConstructEvent(json,
                stripeSignature, endpointSecret2);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
        }

        if (stripeEvent == null)
        {
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json,
                    stripeSignature, endpointSecret);
            }
            catch (Exception i)
            {
                logger.LogError(i.Message, i);
            }
        }

        return stripeEvent;
    }

    private async Task<IActionResult> HandleInvoicePaymentFailed(ControllerBase controller, Invoice paymentIntent)
    {
        PaymentStripeModel? paymentStripeModel = await paymentSesionService.GetSubscriptionPlanIdFromSessionIdAlternative(paymentIntent);

        if (paymentStripeModel == null)
        {
            return controller.BadRequest();
        }

        if (paymentStripeModel.SubscriptionPlainId == Guid.Empty
            || paymentStripeModel.ClinicId == Guid.Empty
            || string.IsNullOrEmpty(paymentStripeModel.StripeCustomerId))
        {
            return controller.BadRequest();
        }

        var subscription = await subscriptionPaymentRepository.GetSubscriptionByStripeCustomerId(paymentStripeModel.StripeCustomerId);

        if (subscription == null)
        {
            logger.LogError($"Dicarting because clinic {paymentStripeModel.ClinicId} with stripe customer id: {paymentStripeModel.StripeCustomerId} , dot not have a subscruption ");
            return controller.BadRequest();
        }

        await subscriptionPaymentRepository.UpdateState(subscription.Id, SubscriptionStates.REJECTED_BY_BANK);

        var charge = new SubscriptionChargeTable()
        {
            Created = DateTime.UtcNow,
            SubscriptionPaymentTableId = subscription.Id,
            IsPaid = false,
            Error = $"Status: {paymentIntent.Status}, CustomerId: {paymentIntent.CustomerId}, ChargeId : {paymentIntent.ChargeId}"
        };
        await subscriptionChargeRepository.AddCharge(charge);

        return controller.Ok();
    }

    private async Task<IActionResult> HandleInvoicePayment(ControllerBase controller, Invoice paymentIntent)
    {
        PaymentStripeModel? paymentStripeModel = await paymentSesionService.GetSubscriptionPlanIdFromSessionIdAlternative(paymentIntent);

        if (paymentStripeModel == null)
        {
            return controller.BadRequest();
        }

        if (paymentStripeModel.SubscriptionPlainId == Guid.Empty
            || paymentStripeModel.ClinicId == Guid.Empty
            || string.IsNullOrEmpty(paymentStripeModel.StripeCustomerId))
        {
            return controller.BadRequest();
        }

        var currentSuscription = await subscriptionPaymentRepository.GetCurrentActiveClinicSubscription(paymentStripeModel.ClinicId);
        if (currentSuscription?.StripeCustomerId == paymentStripeModel.StripeCustomerId)
        {
            logger.LogInformation($"Update session payment before for customer stripe id {paymentStripeModel.StripeCustomerId}");
            var charge = new SubscriptionChargeTable()
            {
                Created = DateTime.UtcNow,
                SubscriptionPaymentTableId = currentSuscription.Id,
                IsPaid = paymentIntent.Paid,
                Amount = paymentIntent.Total,
                StripeChargeId = paymentIntent.ChargeId ?? string.Empty,
            };
            await subscriptionChargeRepository.AddCharge(charge);
            await subscriptionPaymentRepository.UpdateState(currentSuscription.Id, SubscriptionStates.ACTIVE);
        }
        else
        {
            logger.LogInformation($"Create session payment for customer stripe id {paymentStripeModel.StripeCustomerId}");
            Guid subscriptionPAymentId = Guid.NewGuid();
            var subscriptionPayment = new SubscriptionPaymentTable()
            {
                Id = subscriptionPAymentId,
                SubscriptionPlanId = paymentStripeModel.SubscriptionPlainId,
                ClinicId = paymentStripeModel.ClinicId,
                StripeCustomerId = paymentStripeModel.StripeCustomerId,
                StripeSubscriptionId = paymentStripeModel.StripeSubcriptionId,
                State = SubscriptionStates.ACTIVE,
                IsMonthly = paymentStripeModel.IsMonthly,
                IsAnnual = paymentStripeModel.IsAnnual,
                SubscriptionCharges = new List<SubscriptionChargeTable>()
                {
                    new SubscriptionChargeTable()
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.UtcNow,
                        SubscriptionPaymentTableId = subscriptionPAymentId,
                        IsPaid = paymentIntent.Paid,
                        Amount = paymentIntent.Total,
                        StripeChargeId = paymentIntent.ChargeId ?? string.Empty,
                    },
                 }
            };

            await subscriptionPaymentRepository.Create(subscriptionPayment);
        }

        return controller.Ok();
    }
}
