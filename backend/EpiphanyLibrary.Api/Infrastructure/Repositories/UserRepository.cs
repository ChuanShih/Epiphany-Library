using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EpiphanyLibrary.Api.Application.Interfaces;
using EpiphanyLibrary.Api.Domain.Entities;
using EpiphanyLibrary.Api.Infrastructure.Configuration;
using EpiphanyLibrary.Api.Infrastructure.Persistence.MongoDb.Models;

namespace EpiphanyLibrary.Api.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserDocument> _users;

    public UserRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _users = database.GetCollection<UserDocument>(mongoDbSettings.Value.UsersCollectionName);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var doc = await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
        return doc?.ToDomain();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var doc = await _users.Find(x => x.Username == username.ToLowerInvariant()).FirstOrDefaultAsync();
        return doc?.ToDomain();
    }

    public async Task<User> CreateAsync(User user)
    {
        var doc = UserDocument.FromDomain(user);
        await _users.InsertOneAsync(doc);
        return doc.ToDomain();
    }

    public async Task<bool> ExistsAsync(string username)
    {
        var count = await _users.CountDocumentsAsync(x => x.Username == username.ToLowerInvariant());
        return count > 0;
    }
}
