namespace DatabaseAccess.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Name { get; set; }

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public bool? ForcePasswordChange { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AiPrompt> AiPrompts { get; set; } = new List<AiPrompt>();

    public virtual ICollection<AnalysisTip> AnalysisTips { get; set; } = new List<AnalysisTip>();

    public virtual ICollection<BetaRequest> BetaRequests { get; set; } = new List<BetaRequest>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

    public virtual ICollection<TipFeedback> TipFeedbacks { get; set; } = new List<TipFeedback>();

    public virtual ICollection<UserArchetype> UserArchetypes { get; set; } = new List<UserArchetype>();

    public virtual ICollection<UserInvite> UserInvites { get; set; } = new List<UserInvite>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
