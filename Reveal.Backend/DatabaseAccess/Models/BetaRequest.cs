namespace DatabaseAccess.Models;

public partial class BetaRequest
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? ReviewedAt { get; set; }

    public string? ReviewedById { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? ReviewedBy { get; set; }
}
