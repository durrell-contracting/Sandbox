namespace Reveal.DatabaseAccess.Models;

public partial class SkillActionMapping
{
    public string Id { get; set; } = null!;

    public string SkillId { get; set; } = null!;

    public string ActionId { get; set; } = null!;

    public float? Weight { get; set; }

    public virtual Action Action { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
