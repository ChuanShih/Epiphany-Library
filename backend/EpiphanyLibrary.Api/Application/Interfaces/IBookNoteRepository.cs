using EpiphanyLibrary.Api.Domain.Entities;

namespace EpiphanyLibrary.Api.Application.Interfaces;

public interface IBookNoteRepository
{
    Task<IEnumerable<BookNote>> GetAllAsync();
    Task<BookNote?> GetByIdAsync(string id);
    Task<IEnumerable<BookNote>> GetByAuthorIdAsync(string authorId);
    Task<BookNote> CreateAsync(BookNote bookNote);
    Task<BookNote?> UpdateAsync(string id, BookNote bookNote);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}
