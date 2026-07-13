namespace DatabaseAccess.Models;

public partial class Video
{
    public string Id { get; set; } = null!;

    public string GameId { get; set; } = null!;

    public string? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string FileName { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public int? Duration { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? UploadedAt { get; set; }

    public virtual ICollection<Analysis> Analyses { get; set; } = new List<Analysis>();

    public virtual Game Game { get; set; } = null!;

    public virtual User? User { get; set; }

    public virtual ICollection<VideoFrame> VideoFrames { get; set; } = new List<VideoFrame>();

    public virtual ICollection<VideoSegment> VideoSegments { get; set; } = new List<VideoSegment>();

    public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
}
