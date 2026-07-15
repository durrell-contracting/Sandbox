namespace Reveal.DatabaseAccess.Models;

public partial class Game
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? IconUrl { get; set; }

    public string Genre { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual ICollection<AiPrompt> AiPrompts { get; set; } = new List<AiPrompt>();

    public virtual ICollection<GameSkill> GameSkills { get; set; } = new List<GameSkill>();

    public virtual ICollection<TrainingExample> TrainingExamples { get; set; } = new List<TrainingExample>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
