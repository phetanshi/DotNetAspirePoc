using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SkillCentral.Repository;

public static class RepositoryServices
{
    public static void AddSqlServerRepository<T>(this IServiceCollection services, IConfiguration config, string configKeyForConnStr = "AppDbConnection") where T : DbContext
    {
        string connStr = config.GetConnectionString(configKeyForConnStr);

        services.AddScoped<DbContext, T>();
        services.AddScoped<IRepository, Repository>();
        services.AddDbContextPool<T>(options => options.UseSqlServer(connStr));
    }

    public static void AddSqliteRepository<T>(this IServiceCollection services, IConfiguration config, string configKeyForConnStr = "AppDbConnection") where T : DbContext
    {
        string connStr = config.GetConnectionString(configKeyForConnStr);

        services.AddScoped<DbContext, T>();
        services.AddScoped<IRepository, Repository>();
        services.AddDbContextPool<T>(options => options.UseSqlite(connStr));
    }
}
