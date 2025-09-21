namespace EpiphanyLibrary.Api.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // Business rules
    public void UpdateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));

        if (username.Length < 3)
            throw new ArgumentException("Username must be at least 3 characters long", nameof(username));

        Username = username.Trim().ToLowerInvariant();
    }

    public void SetPassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

        PasswordHash = passwordHash;
    }
}
