using FreyaApi.Repository.Services;

namespace FreyaApi.Repository;

public static class RepositoryServices
{
    public static IServiceCollection UseFreyaRepository(this IServiceCollection serviceProvider)
    {
        serviceProvider.AddTransient<IAppManagerRepository, AppManagerRepository>();
        serviceProvider.AddTransient<IClientRepository, ClientRepository>();
        serviceProvider.AddTransient<IClinicRepository, ClinicRepository>();
        serviceProvider.AddTransient<ICompaniesRepository, CompaniesRepository>();
        serviceProvider.AddTransient<IEmployeeRepository, EmployeeRepository>();
        serviceProvider.AddTransient<IFAQRepository, FAQRepository>();
        serviceProvider.AddTransient<ISubscriptionPaymentRepository, SubscriptionPaymentRepository>();
        serviceProvider.AddTransient<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
        serviceProvider.AddTransient<ISubscriptionProductRepository, SubscriptionProductRepository>();
        serviceProvider.AddTransient<ISubscriptionChargeRepository, SubscriptionChargeRepository>();
        serviceProvider.AddTransient<IHelperReposiory, HelperReposiory>();
        serviceProvider.AddTransient<ISoundRepository, SoundRepository>();
        serviceProvider.AddTransient<IImageRepository, ImageRepository>();
        serviceProvider.AddTransient<IVideoRepository, VideoRepository>();

        return serviceProvider;
    }
}
