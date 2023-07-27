namespace FreyaApi.Repository.Tests.Builders;

internal class EmployeeRepositoryBuilder
{
    public EmployeeRepository Build()
    {
        var dbContext = new DatabaseBuilder().Build();

        return new EmployeeRepository(dbContext);
    }
}
