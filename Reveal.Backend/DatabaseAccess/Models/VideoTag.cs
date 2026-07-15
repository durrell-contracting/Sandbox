namespace Reveal.DatabaseAccess.Models;

public partial class VideoTag
{
    public string Id { get; set; } = null!;

    public string VideoId { get; set; } = null!;

    public string? SegmentId { get; set; }

    public string? FrameId { get; set; }

    public string? SkillId { get; set; }

    public string? ActionId { get; set; }

    public float Timestamp { get; set; }

    public float? EndTimestamp { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Source { get; set; }

    public float? Confidence { get; set; }

    public string? Difficulty { get; set; }

    public bool? IsExemplar { get; set; }

    public float? QualityScore { get; set; }

    public int? ConsensusCount { get; set; }

    public string? FrameImageUrl { get; set; }

    public bool? UseForLearning { get; set; }

    public virtual Action? Action { get; set; }

    public virtual VideoFrame? Frame { get; set; }

    public virtual VideoSegment? Segment { get; set; }

    public virtual Skill? Skill { get; set; }

    public virtual ICollection<SkillDetection> SkillDetections { get; set; } = new List<SkillDetection>();

    public virtual ICollection<TrainingExample> TrainingExamples { get; set; } = new List<TrainingExample>();

    public virtual Video Video { get; set; } = null!;
}
