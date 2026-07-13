namespace DatabaseAccess.Models;

public partial class AnalysisTip
{
    public string Id { get; set; } = null!;

    public string AnalysisId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string TipType { get; set; } = null!;

    public string SkillName { get; set; } = null!;

    public float? SkillScore { get; set; }

    public string Content { get; set; } = null!;

    public string Source { get; set; } = null!;

    public string? LibraryTipId { get; set; }

    public string? ResourceUrl { get; set; }

    public string? ResourceLabel { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Analysis Analysis { get; set; } = null!;

    public virtual TipLibrary? LibraryTip { get; set; }

    public virtual ICollection<TipFeedback> TipFeedbacks { get; set; } = new List<TipFeedback>();

    public virtual User User { get; set; } = null!;
}
