namespace FreyaApi.Repository.Interfaces;

public interface IFAQRepository
{
    Task<bool> Create(FAQTable faq);

    Task<bool> Delete(Guid faqId);

    Task<IEnumerable<FAQTable>> GetAllFAQ();
    Task<bool> Update(FAQ request);
}
