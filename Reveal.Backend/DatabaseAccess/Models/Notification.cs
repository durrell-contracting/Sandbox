namespace DatabaseAccess.Models;

public partial class Notification
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? ReferenceId { get; set; }

    public string? VideoId { get; set; }

    public string Title { get; set; } = null!;

    public string? Body { get; set; }

    public bool? Read { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
