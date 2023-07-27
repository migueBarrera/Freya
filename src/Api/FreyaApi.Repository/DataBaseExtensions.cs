namespace FreyaApi.Repository;

public static class DataBaseExtensions
{
    public static IServiceCollection AddDataBaseConfiguration(this IServiceCollection services, IConfiguration config)
    {
        var databaseConfig = config.GetSection("DataBase");
        var userDB = databaseConfig.GetSection("User").Value;
        var passDB = databaseConfig.GetSection("Pass").Value;
        var dataBaseName = databaseConfig.GetSection("DataBaseName").Value;

        var connectionString = $"server=localhost;user={userDB};password={passDB};database={dataBaseName}";

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

        services.AddDbContext<DatabaseContext>(
        dbContextOptions => dbContextOptions
            .UseMySql(connectionString, serverVersion)
            //TODO remove for production
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
    );

        return services;
    }
}
