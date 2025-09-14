using EpiphanyLibrary.Api.Application.DTOs;
using EpiphanyLibrary.Api.Application.Interfaces;
using EpiphanyLibrary.Api.Domain.Entities;

namespace EpiphanyLibrary.Api.Application.Services;

public class BookNoteService : IBookNoteService
{
    private readonly IBookNoteRepository _repository;

    public BookNoteService(IBookNoteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BookNoteDto>> GetAllAsync()
    {
        var bookNotes = await _repository.GetAllAsync();
        return bookNotes.Select(MapToDto);
    }

    public async Task<BookNoteDto?> GetByIdAsync(string id)
    {
        var bookNote = await _repository.GetByIdAsync(id);
        return bookNote != null ? MapToDto(bookNote) : null;
    }

    public async Task<IEnumerable<BookNoteDto>> GetByAuthorIdAsync(string authorId)
    {
        var bookNotes = await _repository.GetByAuthorIdAsync(authorId);
        return bookNotes.Select(MapToDto);
    }

    public async Task<BookNoteDto> CreateAsync(CreateBookNoteDto createDto, string authorId)
    {
        var bookNote = new BookNote();
        bookNote.UpdateContent(createDto.Title, createDto.Content);
        bookNote.AuthorId = authorId;

        var createdBookNote = await _repository.CreateAsync(bookNote);
        return MapToDto(createdBookNote);
    }

    public async Task<BookNoteDto?> UpdateAsync(string id, UpdateBookNoteDto updateDto, string authorId)
    {
        var existingBookNote = await _repository.GetByIdAsync(id);
        if (existingBookNote == null || !existingBookNote.IsAuthoredBy(authorId))
        {
            return null;
        }

        existingBookNote.UpdateContent(updateDto.Title, updateDto.Content);
        var updatedBookNote = await _repository.UpdateAsync(id, existingBookNote);

        return updatedBookNote != null ? MapToDto(updatedBookNote) : null;
    }

    public async Task<bool> DeleteAsync(string id, string authorId)
    {
        var existingBookNote = await _repository.GetByIdAsync(id);
        if (existingBookNote == null || !existingBookNote.IsAuthoredBy(authorId))
        {
            return false;
        }

        return await _repository.DeleteAsync(id);
    }

    private static BookNoteDto MapToDto(BookNote bookNote)
    {
        return new BookNoteDto
        {
            Id = bookNote.Id,
            Title = bookNote.Title,
            Content = bookNote.Content,
            AuthorId = bookNote.AuthorId,
            CreatedAt = bookNote.CreatedAt
        };
    }
}
