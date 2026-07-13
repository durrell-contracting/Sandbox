namespace DatabaseAccess.Models;

public partial class GameSkill
{
    public string Id { get; set; } = null!;

    public string GameId { get; set; } = null!;

    public string SkillId { get; set; } = null!;

    public float? RelevanceScore { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
