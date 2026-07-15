namespace Reveal.DatabaseAccess.Models;

public partial class AiPrompt
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string TaskType { get; set; } = null!;

    public string? GameId { get; set; }

    public string? Model { get; set; }

    public string? SystemPrompt { get; set; }

    public string UserPromptTemplate { get; set; } = null!;

    public int? Priority { get; set; }

    public bool? IsActive { get; set; }

    public int? Version { get; set; }

    public string? UpdatedById { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? UpdatedBy { get; set; }
}
