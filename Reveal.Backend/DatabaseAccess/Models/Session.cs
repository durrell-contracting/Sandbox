namespace Reveal.DatabaseAccess.Models;

public partial class Session
{
    public string Sid { get; set; } = null!;

    public string Sess { get; set; } = null!;

    public DateTime Expire { get; set; }
}
