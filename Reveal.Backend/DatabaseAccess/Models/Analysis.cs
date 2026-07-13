namespace DatabaseAccess.Models;

public partial class Analysis
{
    public string Id { get; set; } = null!;

    public string VideoId { get; set; } = null!;

    public string? SegmentId { get; set; }

    public string Status { get; set; } = null!;

    public string? AnalysisMode { get; set; }

    public string? AiProvider { get; set; }

    public string? Summary { get; set; }

    public string? DetectedActions { get; set; }

    public string? RawResponse { get; set; }

    public bool? IsPinned { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual ICollection<AnalysisTip> AnalysisTips { get; set; } = new List<AnalysisTip>();

    public virtual VideoSegment? Segment { get; set; }

    public virtual ICollection<SkillDetection> SkillDetections { get; set; } = new List<SkillDetection>();

    public virtual Video Video { get; set; } = null!;
}
