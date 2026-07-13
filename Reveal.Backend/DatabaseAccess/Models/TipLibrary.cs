namespace DatabaseAccess.Models;

public partial class TipLibrary
{
    public string Id { get; set; } = null!;

    public string SkillName { get; set; } = null!;

    public string Track { get; set; } = null!;

    public float ScoreMin { get; set; }

    public float ScoreMax { get; set; }

    public string Content { get; set; } = null!;

    public string? ResourceUrl { get; set; }

    public string? ResourceLabel { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AnalysisTip> AnalysisTips { get; set; } = new List<AnalysisTip>();
}
