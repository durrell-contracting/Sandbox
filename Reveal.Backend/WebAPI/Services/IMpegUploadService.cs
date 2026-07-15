using Reveal.CommonObjects.DTOs;

namespace WebAPI.Services;

public interface IMpegUploadService
{
    Task<MpegUploadResult> ProcessMpegUploadAsync(string filename, Stream fileStream);
}
