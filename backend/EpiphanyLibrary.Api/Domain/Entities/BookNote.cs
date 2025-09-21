namespace EpiphanyLibrary.Api.Domain.Entities;

public class BookNote : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string AuthorId { get; set; } = string.Empty;

    // Business rules
    public void UpdateContent(string title, string content)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));

        Title = title.Trim();
        Content = content.Trim();
    }

    public bool IsAuthoredBy(string userId) => AuthorId == userId;
}
