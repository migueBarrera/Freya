using Models.Core.FAQ;

namespace FreyaApi.Infrastructure.Mappers;

public static class FAQMapper
{
    public static FAQ ConvertTo(FAQTable table)
    {
        return new FAQ()
        {
            Id = table.Id,
            Order = table.Order,
            Desc_es = table.Desc_es,
            Title_es = table.Title_es,
            OriginalDesc = table.OriginalDesc,
            OriginalTitle = table.OriginalTitle,
        };
    }

    public static FAQTable ConvertTo(FAQ request)
    {
        return new FAQTable()
        {
            Order = request.Order,
            Desc_es = request.Desc_es,
            Title_es = request.Title_es,
            OriginalDesc = request.OriginalDesc,
            OriginalTitle = request.OriginalTitle,
        };
    }
}
