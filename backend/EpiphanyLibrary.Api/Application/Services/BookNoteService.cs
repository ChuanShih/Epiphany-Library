using EpiphanyLibrary.Api.Application.DTOs;
using EpiphanyLibrary.Api.Application.Interfaces;
using EpiphanyLibrary.Api.Domain.Entities;

namespace EpiphanyLibrary.Api.Application.Services;

public class BookNoteService : IBookNoteService
{
    private readonly IBookNoteRepository _bookRepository;

    public BookNoteService(IBookNoteRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookNoteDto>> GetAllAsync()
    {
        var bookNotes = await _bookRepository.GetAllAsync();
        return bookNotes.Select(MapToDto);
    }

    public async Task<BookNoteDto?> GetByIdAsync(string id)
    {
        var bookNote = await _bookRepository.GetByIdAsync(id);
        return bookNote != null ? MapToDto(bookNote) : null;
    }

    public async Task<IEnumerable<BookNoteDto>> GetByAuthorIdAsync(string authorId)
    {
        var bookNotes = await _bookRepository.GetByAuthorIdAsync(authorId);
        return bookNotes.Select(MapToDto);
    }

    public async Task<BookNoteDto> CreateAsync(CreateBookNoteDto createDto, string authorId)
    {
        var bookNote = new BookNote();
        bookNote.UpdateContent(createDto.Title, createDto.Content);
        bookNote.AuthorId = authorId;

        var createdBookNote = await _bookRepository.CreateAsync(bookNote);
        return MapToDto(createdBookNote);
    }

    public async Task<BookNoteDto?> UpdateAsync(string id, UpdateBookNoteDto updateDto, string authorId)
    {
        var existingBookNote = await _bookRepository.GetByIdAsync(id);
        if (existingBookNote == null || !existingBookNote.IsAuthoredBy(authorId))
        {
            return null;
        }

        existingBookNote.UpdateContent(updateDto.Title, updateDto.Content);
        var updatedBookNote = await _bookRepository.UpdateAsync(id, existingBookNote);

        return updatedBookNote != null ? MapToDto(updatedBookNote) : null;
    }

    public async Task<bool> DeleteAsync(string id, string authorId)
    {
        var existingBookNote = await _bookRepository.GetByIdAsync(id);
        if (existingBookNote == null || !existingBookNote.IsAuthoredBy(authorId))
        {
            return false;
        }

        return await _bookRepository.DeleteAsync(id);
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
