using Reveal.CommonObjects.DTOs;

namespace Reveal.DatabaseAccess.Services;

public interface IDatabaseService
{
    Task<string> CreateMpegFileRecordAsync(MpegFileRecord record);
    Task UpdateMpegFileRecordAsync(string recordId, MpegFileRecord record);
    Task<MpegFileRecord> GetMpegFileRecordAsync(string recordId);
    Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default);
    Task InitializeDatabaseAsync(CancellationToken cancellationToken = default);
}
