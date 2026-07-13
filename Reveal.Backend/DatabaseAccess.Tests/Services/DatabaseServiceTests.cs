using DatabaseAccess.Models;
using DatabaseAccess.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess.Tests.Services;

public class DatabaseServiceTests : IDisposable
{
    private readonly DbContextOptions<RevealContext> _options;
    private readonly RevealContext _context;

    public DatabaseServiceTests()
    {
        _options = new DbContextOptionsBuilder<RevealContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new RevealContext(_options);
    }

    [Fact]
    public async Task HealthCheckAsync_WithValidContext_ReturnsTrue()
    {
        // Arrange
        var service = new DatabaseService(_context);

        // Act
        var result = await service.HealthCheckAsync();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task HealthCheckAsync_WithValidContext_DoesNotThrow()
    {
        // Arrange
        var service = new DatabaseService(_context);

        // Act & Assert
        var exception = await Record.ExceptionAsync(
            () => service.HealthCheckAsync(CancellationToken.None));

        exception.Should().BeNull();
    }

    [Fact]
    public async Task InitializeDatabaseAsync_WithValidContext_CompletesSuccessfully()
    {
        // Arrange
        var service = new DatabaseService(new RevealContext());

        // Act & Assert
        var exception = await Record.ExceptionAsync(
            () => service.InitializeDatabaseAsync(CancellationToken.None));

        exception.Should().BeNull();
    }

    [Fact]
    public async Task HealthCheckAsync_CanBeCancelledWithToken()
    {
        // Arrange
        var service = new DatabaseService(_context);
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Act
        var result = await service.HealthCheckAsync(cts.Token);

        // Assert
        result.Should().BeTrue();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}