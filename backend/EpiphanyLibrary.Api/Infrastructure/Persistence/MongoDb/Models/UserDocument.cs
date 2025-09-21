using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using EpiphanyLibrary.Api.Domain.Entities;

namespace EpiphanyLibrary.Api.Infrastructure.Persistence.MongoDb.Models;

public class UserDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;

    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    // Mapping: Domain -> Document
    public static UserDocument FromDomain(User user) =>
        new()
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            CreatedAt = user.CreatedAt
        };

    // Mapping: Document -> Domain
    public User ToDomain() =>
        new()
        {
            Id = this.Id,
            Username = this.Username,
            PasswordHash = this.PasswordHash,
            CreatedAt = this.CreatedAt
        };
}
