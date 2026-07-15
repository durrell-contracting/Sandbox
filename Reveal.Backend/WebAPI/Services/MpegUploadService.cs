using Reveal.CommonObjects.DTOs;
using Reveal.DatabaseAccess.Services;
using Reveal.MessageBroker.Services;
using Reveal.ObjectStorage.Services;
using WebAPI.Services;

namespace Reveal.WebAPI.Services;

public class MpegUploadService : IMpegUploadService
{
    private readonly IObjectStorageService _objectStorageService;
    private readonly IDatabaseService _databaseService;
    private readonly IMessageBrokerService _messageBusService;

    public MpegUploadService(
        IObjectStorageService objectStorageService,
        IDatabaseService databaseService,
        IMessageBrokerService messageBusService)
    {
        _objectStorageService = objectStorageService ?? throw new ArgumentNullException(nameof(objectStorageService));
        _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        _messageBusService = messageBusService ?? throw new ArgumentNullException(nameof(messageBusService));
    }

    public async Task<MpegUploadResult> ProcessMpegUploadAsync(string filename, Stream fileStream)
    {
        try
        {
            var fileId = Guid.NewGuid().ToString();
            var contentType = GetMimeType(filename);

            // Step 1: Upload to object storage (MinIO)
            var storageLocation = await _objectStorageService.UploadFileAsync(
                fileId,
                fileStream,
                contentType);

            // Step 2: Create database record
            var fileRecord = new MpegFileRecord
            {
                Id = fileId,
                OriginalFileName = filename,
                StorageKey = storageLocation,
                FileSizeBytes = fileStream.Length,
                MimeType = contentType,
                UploadedAt = DateTime.UtcNow,
                Status = "Uploaded"
            };

            var databaseRecordId = await _databaseService.CreateMpegFileRecordAsync(fileRecord);

            // Step 3: Publish to message bus
            await _messageBusService.PublishMpegUploadedEventAsync(
                fileId,
                filename,
                storageLocation);

            return new MpegUploadResult
            {
                Success = true,
                FileId = fileId,
                StorageLocation = storageLocation,
                DatabaseRecordId = databaseRecordId,
                UploadedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            return new MpegUploadResult
            {
                Success = false,
                ErrorMessage = $"Error processing MPEG upload: {ex.Message}"
            };
        }
    }

    private static string GetMimeType(string filename)
    {
        var extension = Path.GetExtension(filename).ToLowerInvariant();
        return extension switch
        {
            ".mp3" => "audio/mpeg",
            ".mp4" => "video/mp4",
            ".mpeg" => "video/mpeg",
            ".mpg" => "video/mpeg",
            _ => "application/octet-stream"
        };
    }
}