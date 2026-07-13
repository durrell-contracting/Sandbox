using DatabaseAccess.Configuration;
using DatabaseAccess.Models;
using DatabaseAccess.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseAccess(
        this IServiceCollection services,
        DatabaseConfig? config = null)
    {
        config ??= new DatabaseConfig();

        services.AddScoped(_ => config);

        services.AddDbContext<RevealContext>(options =>
            options.UseNpgsql(config.GetConnectionString()));

        services.AddScoped<IDatabaseService, DatabaseService>();

        return services;
    }
}