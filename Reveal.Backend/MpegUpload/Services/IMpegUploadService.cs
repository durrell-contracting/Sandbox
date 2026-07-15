using Reveal.CommonObjects.DTOs;

namespace Reveal.MpegUpload.Services;

public interface IMpegUploadService
{
    Task<MpegUploadResult> ProcessMpegUploadAsync(string filename, Stream fileStream);
}
