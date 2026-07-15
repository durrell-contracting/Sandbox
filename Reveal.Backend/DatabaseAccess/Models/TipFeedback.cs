namespace Reveal.DatabaseAccess.Models;

public partial class TipFeedback
{
    public string Id { get; set; } = null!;

    public string TipId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string Rating { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual AnalysisTip Tip { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
