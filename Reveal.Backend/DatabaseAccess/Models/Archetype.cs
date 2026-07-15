namespace Reveal.DatabaseAccess.Models;

public partial class Archetype
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Tagline { get; set; }

    public string SkillWeights { get; set; } = null!;

    public string Colour { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public bool? IsActive { get; set; }

    public int? SortOrder { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<UserArchetype> UserArchetypes { get; set; } = new List<UserArchetype>();
}
