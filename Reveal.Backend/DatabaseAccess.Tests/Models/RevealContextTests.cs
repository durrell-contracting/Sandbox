using Reveal.DatabaseAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Reveal.DatabaseAccess.Tests.Models;

public class RevealContextTests : IDisposable
{
    private readonly DbContextOptions<RevealContext> _options;

    public RevealContextTests()
    {
        _options = new DbContextOptionsBuilder<RevealContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void Constructor_WithValidOptions_CreatesInstance()
    {
        // Arrange & Act
        using var context = new RevealContext(_options);

        // Assert
        Assert.NotNull(context);
    }

    [Fact]
    public async Task Database_CanConnectAsync_WithValidContext()
    {
        // Arrange
        using var context = new RevealContext(_options);

        // Act
        var canConnect = await context.Database.CanConnectAsync();

        // Assert
        Assert.True(canConnect);
    }

    public void Dispose()
    {
        using var context = new RevealContext(_options);
        context.Database.EnsureDeleted();
    }
}