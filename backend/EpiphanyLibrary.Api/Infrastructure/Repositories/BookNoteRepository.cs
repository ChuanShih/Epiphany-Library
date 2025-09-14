using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EpiphanyLibrary.Api.Application.Interfaces;
using EpiphanyLibrary.Api.Domain.Entities;
using EpiphanyLibrary.Api.Infrastructure.Configuration;

namespace EpiphanyLibrary.Api.Infrastructure.Repositories;

public class BookNoteRepository : IBookNoteRepository
{
    private readonly IMongoCollection<BookNote> _bookNotes;

    public BookNoteRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _bookNotes = database.GetCollection<BookNote>(mongoDbSettings.Value.BookNotesCollectionName);
    }

    public async Task<IEnumerable<BookNote>> GetAllAsync()
    {
        return await _bookNotes.Find(_ => true).ToListAsync();
    }

    public async Task<BookNote?> GetByIdAsync(string id)
    {
        return await _bookNotes.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<BookNote>> GetByAuthorIdAsync(string authorId)
    {
        return await _bookNotes.Find(x => x.AuthorId == authorId).ToListAsync();
    }

    public async Task<BookNote> CreateAsync(BookNote bookNote)
    {
        await _bookNotes.InsertOneAsync(bookNote);
        return bookNote;
    }

    public async Task<BookNote?> UpdateAsync(string id, BookNote bookNote)
    {
        var result = await _bookNotes.ReplaceOneAsync(x => x.Id == id, bookNote);
        return result.ModifiedCount > 0 ? bookNote : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _bookNotes.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var count = await _bookNotes.CountDocumentsAsync(x => x.Id == id);
        return count > 0;
    }
}
