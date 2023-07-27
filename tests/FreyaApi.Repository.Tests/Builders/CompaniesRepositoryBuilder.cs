namespace FreyaApi.Repository.Tests.Builders;

internal class CompaniesRepositoryBuilder
{
    private List<CompanyTable> listCompanies = new List<CompanyTable>();

    public CompaniesRepository Build()
    {
        var dbContext = new DatabaseBuilder().Build();
        foreach (var item in listCompanies)
        {
            dbContext.Company?.Add(item);
            dbContext.SaveChanges();
        }

        return new CompaniesRepository(dbContext);
    }

    internal CompaniesRepositoryBuilder WithCompany(string v)
    {
        listCompanies.Add(new CompanyTable() { Name = v });
        return this;
    }
    
    internal CompaniesRepositoryBuilder WithCompany(string name, Guid id)
    {
        listCompanies.Add(new CompanyTable() { Name = name, Id = id });
        return this;
    }
}
