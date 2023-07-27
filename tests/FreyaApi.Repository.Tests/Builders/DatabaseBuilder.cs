using Microsoft.Extensions.Logging;

namespace FreyaApi.Repository.Tests.Builders;

internal class DatabaseBuilder
{
    internal DatabaseContext Build()
    {
        var generatedName = Guid.NewGuid().ToString();
        var dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(generatedName)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .Options;

        return new DatabaseContext(dbContextOptions);
    }
}
