using EpiphanyLibrary.Api.Application.DTOs;

namespace EpiphanyLibrary.Api.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    string GenerateJwtToken(string userId, string username);
}
