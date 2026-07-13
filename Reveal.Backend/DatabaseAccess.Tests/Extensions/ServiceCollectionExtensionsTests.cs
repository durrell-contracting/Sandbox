using DatabaseAccess.Configuration;
using DatabaseAccess.Extensions;
using DatabaseAccess.Models;
using DatabaseAccess.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseAccess.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDatabaseAccess_WithoutConfig_RegistersServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDatabaseAccess();
        var provider = services.BuildServiceProvider();

        // Assert
        var dbContext = provider.GetRequiredService<RevealContext>();
        var databaseService = provider.GetRequiredService<IDatabaseService>();
        var config = provider.GetRequiredService<DatabaseConfig>();

        dbContext.Should().NotBeNull();
        databaseService.Should().NotBeNull();
        config.Should().NotBeNull();
    }

    [Fact]
    public void AddDatabaseAccess_WithConfig_RegistersServicesWithCustomConfig()
    {
        // Arrange
        var services = new ServiceCollection();
        var customConfig = new DatabaseConfig
        {
            Host = "custom-host",
            Port = 5433,
            Username = "custom-user",
            Password = "custom-pass",
            Database = "custom_db"
        };

        // Act
        services.AddDatabaseAccess(customConfig);
        var provider = services.BuildServiceProvider();

        // Assert
        var retrievedConfig = provider.GetRequiredService<DatabaseConfig>();
        retrievedConfig.Should().Be(customConfig);
        retrievedConfig.Host.Should().Be("custom-host");
        retrievedConfig.Port.Should().Be(5433);
    }

    [Fact]
    public void AddDatabaseAccess_RegistersRevealContextAsScoped()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDatabaseAccess();
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(RevealContext));

        // Assert
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddDatabaseAccess_RegistersDatabaseServiceAsScoped()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDatabaseAccess();
        var descriptor = services.FirstOrDefault(
            d => d.ServiceType == typeof(IDatabaseService) && 
                 d.ImplementationType == typeof(DatabaseService));

        // Assert
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }
}