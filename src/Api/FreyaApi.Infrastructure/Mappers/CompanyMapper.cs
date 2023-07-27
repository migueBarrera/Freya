using Models.Core.Companies;

namespace FreyaApi.Infrastructure.Mappers;

public static class CompanyMapper
{
    public static Company ConvertTo(CompanyTable table)
    {
        return new Company()
        {
            Id = table.Id,
            Name = table.Name,
            Created = table.Created,
        };
    }
}
