using ApiContract.Interfaces;
using Models.Core.FAQ;

namespace FreyaManager.Features.Faqs;

public class FaqService
{
    private readonly IFaqAPIService faqAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILogger<FaqService> logger;

    public FaqService(
        IRefitService refitService, 
        ITaskHelperFactory taskHelperFactory, 
        ILogger<FaqService> logger)
    {
        this.faqAPIService = refitService.InitRefitInstance<IFaqAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
    }

    public async Task<bool> Create(NewFaqModel newFaq)
    {
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               TryExecuteAsync(
                               () => faqAPIService.Create(newFaq.ConvertTo()));

        return result.IsSuccess;
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               TryExecuteAsync(
                               () => faqAPIService.Delete(id));

        return result.IsSuccess;
    }
    
    public async Task<bool> Update(NewFaqModel newFaq)
    {
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               TryExecuteAsync(
                               () => faqAPIService.Update(newFaq.ConvertTo()));

        return result.IsSuccess;
    }
    
    public async Task<IEnumerable<FAQ>> GetAll()
    {
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               TryExecuteAsync(
                               () => faqAPIService.GetAll());

        if(result.IsSuccess)
        {
            return result.Value;
        }

        return Enumerable.Empty<FAQ>();
    }
}
