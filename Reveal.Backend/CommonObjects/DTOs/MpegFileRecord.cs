namespace Reveal.CommonObjects.DTOs;

public class MpegFileRecord
{
    public string Id { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.MinValue;
    public string UploadedBy { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
