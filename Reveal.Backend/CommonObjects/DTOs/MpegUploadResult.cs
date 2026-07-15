namespace Reveal.CommonObjects.DTOs;

public class MpegUploadResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string FileId { get; set; } = string.Empty;
    public string StorageLocation { get; set; } = string.Empty;
    public string DatabaseRecordId { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.MinValue;
}
