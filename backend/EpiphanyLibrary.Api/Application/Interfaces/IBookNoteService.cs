using EpiphanyLibrary.Api.Application.DTOs;

namespace EpiphanyLibrary.Api.Application.Interfaces;

public interface IBookNoteService
{
    Task<IEnumerable<BookNoteDto>> GetAllAsync();
    Task<BookNoteDto?> GetByIdAsync(string id);
    Task<IEnumerable<BookNoteDto>> GetByAuthorIdAsync(string authorId);
    Task<BookNoteDto> CreateAsync(CreateBookNoteDto createDto, string authorId);
    Task<BookNoteDto?> UpdateAsync(string id, UpdateBookNoteDto updateDto, string authorId);
    Task<bool> DeleteAsync(string id, string authorId);
}
