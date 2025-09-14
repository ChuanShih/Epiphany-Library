using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EpiphanyLibrary.Api.Application.Interfaces;
using EpiphanyLibrary.Api.Domain.Entities;
using EpiphanyLibrary.Api.Infrastructure.Configuration;

namespace EpiphanyLibrary.Api.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _users = database.GetCollection<User>(mongoDbSettings.Value.UsersCollectionName);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _users.Find(x => x.Username == username.ToLowerInvariant()).FirstOrDefaultAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<bool> ExistsAsync(string username)
    {
        var count = await _users.CountDocumentsAsync(x => x.Username == username.ToLowerInvariant());
        return count > 0;
    }
}
