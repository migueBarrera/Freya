namespace FreyaApi.Repository.Services;

public class SubscriptionPaymentRepository : BaseRepository, ISubscriptionPaymentRepository
{
    public SubscriptionPaymentRepository(DatabaseContext databaseContext)
         : base(databaseContext)
    {
    }

    public async Task<bool> Create(SubscriptionPaymentTable subscription)
    {
        context.SubscriptionPayments?.Add(subscription);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<SubscriptionPaymentTable?> GetCurrentActiveClinicSubscription(Guid clinicId)
    {
        var paymentsPlan = await context
                .SubscriptionPayments?
                .Include(sp => sp.SubscriptionPlan)
                        .ThenInclude(sp => sp!.SubscriptionProduct)
                 .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ClinicId == clinicId && p.State == SubscriptionStates.ACTIVE)!;

        return paymentsPlan;
    }

    public async Task<IEnumerable<SubscriptionPaymentTable>> GetPaymentsByClinic(Guid id)
    {
        var paymentsPlans = await context
                    .SubscriptionPayments?
                    .Where(p => p.ClinicId == id)
                    .OrderByDescending(p => p.Created)
                    .Include(sp => sp.SubscriptionPlan)
                        .ThenInclude(sp => sp!.SubscriptionProduct)
                    .AsNoTracking()
                    .ToListAsync()!;

        return paymentsPlans ?? Enumerable.Empty<SubscriptionPaymentTable>();
    }

    public async Task<SubscriptionPaymentTable?> GetSubscriptionByStripeCustomerId(string stripeCustomerId)
    {
        var paymentsPlan = await context
                .SubscriptionPayments?
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.StripeCustomerId == stripeCustomerId)!;

        return paymentsPlan;
    }

    public async Task<SubscriptionPaymentTable?> GetSubscriptionPayment(Guid subscriptioPaymentId)
    {
        var paymentsPlan = await context
                .SubscriptionPayments?
                .Include(sp => sp.SubscriptionCharges.OrderByDescending(x => x.Created))
                .Include(sp => sp.SubscriptionPlan)
                        .ThenInclude(sp => sp!.SubscriptionProduct)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == subscriptioPaymentId)!;

        return paymentsPlan;
    }

    public async Task<bool> UpdateState(Guid subscriptionId, SubscriptionStates newState)
    {
        var paymentsPlan = await context
                .SubscriptionPayments?
                .FirstOrDefaultAsync(p => p.Id == subscriptionId)!;

        if (paymentsPlan == null)
        {
            return false;
        }

        paymentsPlan.State = newState;
        if (newState == SubscriptionStates.REJECTED_BY_EMPLOYEE 
            || newState == SubscriptionStates.REJECTED_BY_BANK)
        {
            paymentsPlan.Expire = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
        return true;
    }
}
