namespace DatabaseAccess.Models;

public partial class VideoSegment
{
    public string Id { get; set; } = null!;

    public string VideoId { get; set; } = null!;

    public float StartTime { get; set; }

    public float EndTime { get; set; }

    public string? Label { get; set; }

    public float? InterestScore { get; set; }

    public string? AiSummary { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? Metadata { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Analysis> Analyses { get; set; } = new List<Analysis>();

    public virtual Video Video { get; set; } = null!;

    public virtual ICollection<VideoFrame> VideoFrames { get; set; } = new List<VideoFrame>();

    public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
}
