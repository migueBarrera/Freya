namespace FreyaApi.Repository.Interfaces;

public interface ISubscriptionChargeRepository
{
    Task AddCharge(SubscriptionChargeTable charge);
}
