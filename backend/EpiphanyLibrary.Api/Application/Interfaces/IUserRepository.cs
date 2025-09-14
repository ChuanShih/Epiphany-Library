using EpiphanyLibrary.Api.Domain.Entities;

namespace EpiphanyLibrary.Api.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> CreateAsync(User user);
    Task<bool> ExistsAsync(string username);
}
