using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EpiphanyLibrary.Api.Application.Interfaces;
using EpiphanyLibrary.Api.Domain.Entities;
using EpiphanyLibrary.Api.Infrastructure.Configuration;
using EpiphanyLibrary.Api.Infrastructure.Persistence.MongoDb.Models;

namespace EpiphanyLibrary.Api.Infrastructure.Repositories;

public class BookNoteRepository : IBookNoteRepository
{
    private readonly IMongoCollection<BookNoteDocument> _bookNotes;

    public BookNoteRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _bookNotes = database.GetCollection<BookNoteDocument>(mongoDbSettings.Value.BookNotesCollectionName);
    }

    public async Task<IEnumerable<BookNote>> GetAllAsync()
    {
        var docs = await _bookNotes.Find(_ => true).ToListAsync();
        return docs.Select(d => d.ToDomain());
    }

    public async Task<BookNote?> GetByIdAsync(string id)
    {
        var doc = await _bookNotes.Find(x => x.Id == id).FirstOrDefaultAsync();
        return doc?.ToDomain();
    }

    public async Task<IEnumerable<BookNote>> GetByAuthorIdAsync(string authorId)
    {
        var docs = await _bookNotes.Find(x => x.AuthorId == authorId).ToListAsync();
        return docs.Select(d => d.ToDomain());
    }

    public async Task<BookNote> CreateAsync(BookNote bookNote)
    {
        var doc = BookNoteDocument.FromDomain(bookNote);
        await _bookNotes.InsertOneAsync(doc);
        return doc.ToDomain();
    }

    public async Task<BookNote?> UpdateAsync(string id, BookNote bookNote)
    {
        var doc = BookNoteDocument.FromDomain(bookNote);
        var result = await _bookNotes.ReplaceOneAsync(x => x.Id == id, doc);
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
