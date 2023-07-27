namespace FreyaApi.Repository.Services;

public class SubscriptionPlanRepository : BaseRepository, ISubscriptionPlanRepository
{
    public SubscriptionPlanRepository(DatabaseContext databaseContext)
         : base(databaseContext)
    {
    }

    public async Task CreateSubscriptionPlans(List<SubscriptionPlanTable> listSubscriptionPlan)
    {
        context.AddRange(listSubscriptionPlan);

        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<SubscriptionPlanTable>> GetPlansForCompany(Guid companyId)
    {
        var paymentsPlans = await context
                .SubscriptionPlans?
                .Where(p => p.CompanyId == companyId && p.IsActive)
                .Include(sp => sp.SubscriptionProduct)
                .AsNoTracking()
                .ToListAsync()!;

        return paymentsPlans ?? Enumerable.Empty<SubscriptionPlanTable>();
    }
}
