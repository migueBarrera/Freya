namespace FreyaApi.Repository.Services;

public class FAQRepository : BaseRepository, IFAQRepository
{
    public FAQRepository(DatabaseContext databaseContext)
         : base(databaseContext)
    {
    }

    public async Task<bool> Create(FAQTable faq)
    {
        context.Add(faq);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(Guid faqId)
    {
        var faqTable = await context.FAQ?
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == faqId)!;

        if (faqTable == null)
        {
            return false;
        }

        context.FAQ.Remove(faqTable);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<FAQTable>> GetAllFAQ()
    {
        var items = await context
            .FAQ?
            .AsNoTracking()
            .OrderBy(c => c.Order)
            .ToListAsync()!;

        return items ?? Enumerable.Empty<FAQTable>();
    }

    public async Task<bool> Update(FAQ request)
    {
        var faqTable = await context
            .FAQ?
            .FirstOrDefaultAsync(x => x.Id == request.Id)!;

        if (faqTable == null)
        {
            return false;
        }

        faqTable.Order = request.Order;
        faqTable.OriginalDesc = request.OriginalDesc;
        faqTable.OriginalTitle = request.OriginalTitle;
        faqTable.Desc_es = request.Desc_es;
        faqTable.Title_es = request.Title_es;

        await context.SaveChangesAsync();
        return true;
    }
}
