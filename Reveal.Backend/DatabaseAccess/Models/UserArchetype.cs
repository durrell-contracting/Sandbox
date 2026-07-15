namespace Reveal.DatabaseAccess.Models;

public partial class UserArchetype
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string ArchetypeId { get; set; } = null!;

    public float Score { get; set; }

    public string? TopSkills { get; set; }

    public int? VideoCount { get; set; }

    public DateTime? ComputedAt { get; set; }

    public virtual Archetype Archetype { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
