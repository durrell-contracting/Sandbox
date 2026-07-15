namespace Reveal.CommonObjects.Messages;

public class MpegUploadedEvent
{
    public string FileId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string StorageLocation { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.MinValue;
}
