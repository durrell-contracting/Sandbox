namespace Reveal.DatabaseAccess.Configuration;

public class DatabaseConfig
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "password";
    public string Database { get; set; } = "reveal";

    public string ConnectionString =>
        $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database}";
}
