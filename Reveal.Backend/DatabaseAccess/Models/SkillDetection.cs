namespace DatabaseAccess.Models;

public partial class SkillDetection
{
    public string Id { get; set; } = null!;

    public string AnalysisId { get; set; } = null!;

    public string SkillId { get; set; } = null!;

    public float ConfidenceScore { get; set; }

    public float? Timestamp { get; set; }

    public string? Evidence { get; set; }

    public string? FrameId { get; set; }

    public string? TagId { get; set; }

    public string? Source { get; set; }

    public bool? IsCandidate { get; set; }

    public bool? HumanVerified { get; set; }

    public float? HumanConfidence { get; set; }

    public bool? IsCorrect { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public string? ReviewerNotes { get; set; }

    public virtual Analysis Analysis { get; set; } = null!;

    public virtual VideoFrame? Frame { get; set; }

    public virtual Skill Skill { get; set; } = null!;

    public virtual VideoTag? Tag { get; set; }
}
