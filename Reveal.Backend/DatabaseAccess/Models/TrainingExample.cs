namespace DatabaseAccess.Models;

public partial class TrainingExample
{
    public string Id { get; set; } = null!;

    public string GameId { get; set; } = null!;

    public string? SkillId { get; set; }

    public string? ActionId { get; set; }

    public string? SourceTagId { get; set; }

    public string FrameImageUrl { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? ContextNotes { get; set; }

    public float QualityScore { get; set; }

    public int? UseCount { get; set; }

    public float? SuccessRate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Action? Action { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Skill? Skill { get; set; }

    public virtual VideoTag? SourceTag { get; set; }
}
