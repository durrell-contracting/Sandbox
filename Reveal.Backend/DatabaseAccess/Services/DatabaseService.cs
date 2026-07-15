using Reveal.DatabaseAccess.Models;
using Microsoft.EntityFrameworkCore;
using Reveal.CommonObjects.DTOs;

namespace Reveal.DatabaseAccess.Services;

public class DatabaseService : IDatabaseService
{
    private readonly RevealContext _context;

    public DatabaseService(RevealContext context)
    {
        _context = context;
    }

    public Task<string> CreateMpegFileRecordAsync(MpegFileRecord record)
    {
        throw new NotImplementedException();
    }

    public Task<MpegFileRecord> GetMpegFileRecordAsync(string recordId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateMpegFileRecordAsync(string recordId, MpegFileRecord record)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Database.CanConnectAsync(cancellationToken);
        }
        catch
        {
            return false;
        }
    }

    public async Task InitializeDatabaseAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.MigrateAsync(cancellationToken);
    }
}