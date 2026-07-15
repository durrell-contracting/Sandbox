namespace Reveal.DatabaseAccess.Models;

public partial class Action
{
    public string Id { get; set; } = null!;

    public string GameId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Category { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual ICollection<SkillActionMapping> SkillActionMappings { get; set; } = new List<SkillActionMapping>();

    public virtual ICollection<TrainingExample> TrainingExamples { get; set; } = new List<TrainingExample>();

    public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
}
