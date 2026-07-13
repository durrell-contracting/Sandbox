namespace DatabaseAccess.Models;

public partial class Skill
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Type { get; set; } = null!;

    public string Category { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<GameSkill> GameSkills { get; set; } = new List<GameSkill>();

    public virtual ICollection<SkillActionMapping> SkillActionMappings { get; set; } = new List<SkillActionMapping>();

    public virtual ICollection<SkillDetection> SkillDetections { get; set; } = new List<SkillDetection>();

    public virtual ICollection<TrainingExample> TrainingExamples { get; set; } = new List<TrainingExample>();

    public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
}
