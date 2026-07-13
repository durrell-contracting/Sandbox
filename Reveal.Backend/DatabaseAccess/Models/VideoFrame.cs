namespace DatabaseAccess.Models;

public partial class VideoFrame
{
    public string Id { get; set; } = null!;

    public string VideoId { get; set; } = null!;

    public string? SegmentId { get; set; }

    public int FrameNumber { get; set; }

    public float Timestamp { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual VideoSegment? Segment { get; set; }

    public virtual ICollection<SkillDetection> SkillDetections { get; set; } = new List<SkillDetection>();

    public virtual Video Video { get; set; } = null!;

    public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
}
