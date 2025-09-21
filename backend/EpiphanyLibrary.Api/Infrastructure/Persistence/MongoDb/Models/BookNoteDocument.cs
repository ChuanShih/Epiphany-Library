using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using EpiphanyLibrary.Api.Domain.Entities;

namespace EpiphanyLibrary.Api.Infrastructure.Persistence.MongoDb.Models;

public class BookNoteDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    [BsonElement("authorId")]
    public string AuthorId { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    // 映射方法 (Domain <-> Mongo)
    public static BookNoteDocument FromDomain(BookNote note) =>
        new()
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            AuthorId = note.AuthorId,
            CreatedAt = note.CreatedAt
        };

    public BookNote ToDomain() =>
        new()
        {
            Id = this.Id,
            Title = this.Title,
            Content = this.Content,
            AuthorId = this.AuthorId,
            CreatedAt = this.CreatedAt
        };
}
