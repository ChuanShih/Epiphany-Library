using Microsoft.AspNetCore.Mvc;
using EpiphanyLibrary.Api.Application.DTOs;
using EpiphanyLibrary.Api.Application.Interfaces;

namespace EpiphanyLibrary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        if (string.IsNullOrWhiteSpace(registerDto.Username) || string.IsNullOrWhiteSpace(registerDto.Password))
        {
            return BadRequest("Username and password are required");
        }

        if (registerDto.Password.Length < 6)
        {
            return BadRequest("Password must be at least 6 characters long");
        }

        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            if (result == null)
            {
                return BadRequest("Username already exists");
            }

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
        {
            return BadRequest("Username and password are required");
        }

        var result = await _authService.LoginAsync(loginDto);
        if (result == null)
        {
            return BadRequest("Invalid username or password");
        }

        return Ok(result);
    }
}
