using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reveal.DatabaseAccess.Configuration;
using Reveal.DatabaseAccess.Models;
using Reveal.DatabaseAccess.Services;

namespace Reveal.DatabaseAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseAccess(
        this IServiceCollection services,
        DatabaseConfig? config = null)
    {
        config ??= new DatabaseConfig();

        services.AddScoped(_ => config);

        services.AddDbContext<RevealContext>(options =>
            options.UseNpgsql(config.ConnectionString));

        services.AddScoped<IDatabaseService, DatabaseService>();

        return services;
    }
}