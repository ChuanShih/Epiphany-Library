namespace EpiphanyLibrary.Api.Domain.DTOs;

public record CreateBookNoteDto(string Title, string Content);

public record UpdateBookNoteDto(string Title, string Content);

public record BookNoteDto(string Id, string Title, string Content, string AuthorId, DateTime CreatedAt);
