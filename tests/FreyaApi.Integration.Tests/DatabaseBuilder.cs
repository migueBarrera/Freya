using MiBebeApi.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreyaApi.Integration.Tests;

public class DatabaseBuilder
{
    public DatabaseContext Build() 
    {
        var databaseOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseMemoryCache(ME)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();

        var db = new DatabaseContext(databaseOptions.Options);

        return db;
    }
}
