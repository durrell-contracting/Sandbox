namespace Reveal.DatabaseAccess.Models;

public partial class UserInvite
{
    public string Id { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Email { get; set; }

    public string Role { get; set; } = null!;

    public int? MaxUses { get; set; }

    public int? UsedCount { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public string? CreatedById { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? CreatedBy { get; set; }
}
