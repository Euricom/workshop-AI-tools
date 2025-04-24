namespace MyApp.Core.Entities;

public class User
{
    public string Id { get; private set; } = default!;
    public string Username { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }

    private User() { } // For EF Core

    public static User Create(string username, string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };
    }
}