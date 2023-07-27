namespace FreyaApi.Repository.Services;

public class SubscriptionChargeRepository : BaseRepository, 
    ISubscriptionChargeRepository
{
    public SubscriptionChargeRepository(DatabaseContext context) : base(context)
    {

    }

    public async Task AddCharge(SubscriptionChargeTable charge)
    {
        await context.AddAsync(charge);

        await context.SaveChangesAsync();
    }
}
