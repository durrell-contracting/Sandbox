namespace DatabaseAccess.Services;

public interface IDatabaseService
{
    Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default);
    Task InitializeDatabaseAsync(CancellationToken cancellationToken = default);
}
