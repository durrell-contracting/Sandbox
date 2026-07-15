using FluentAssertions;
using Reveal.DatabaseAccess.Configuration;

namespace Reveal.DatabaseAccess.Tests.Configuration;

public class DatabaseConfigTests
{
    [Fact]
    public void GetConnectionString_WithDefaults_ReturnsValidConnectionString()
    {
        // Arrange
        var config = new DatabaseConfig();
        var expected = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=reveal";

        // Act
        var result = config.ConnectionString;

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GetConnectionString_WithCustomValues_ReturnsCustomConnectionString()
    {
        // Arrange
        var config = new DatabaseConfig
        {
            Host = "prod-db.example.com",
            Port = 5433,
            Username = "admin",
            Password = "secret",
            Database = "production_db"
        };
        var expected = "Host=prod-db.example.com;Port=5433;Username=admin;Password=secret;Database=production_db";

        // Act
        var result = config.ConnectionString;

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void DatabaseConfig_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var config = new DatabaseConfig();

        // Assert
        config.Host.Should().Be("localhost");
        config.Port.Should().Be(5432);
        config.Username.Should().Be("postgres");
        config.Password.Should().Be("password");
        config.Database.Should().Be("reveal");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void GetConnectionString_WithWhitespaceHost_IncludesHost(string host)
    {
        // Arrange
        var config = new DatabaseConfig { Host = host };

        // Act
        var result = config.ConnectionString;

        // Assert
        result.Should().Contain("Host=");
    }
}