using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using ApplicationCore.Dto;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendLab01.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) =>
        _authService = authService;

    /// <summary>Logowanie — zwraca access token i refresh token.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
        }
        catch (System.Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>Odświeżenie access tokenu.</summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
    {
        try
        {
            var response = await _authService.RefreshTokenAsync(dto);
            return Ok(response);
        }
        catch (System.Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>Wylogowanie — unieważnia refresh token.</summary>
    [HttpPost("revoke")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Revoke([FromBody] string refreshToken)
    {
        try
        {
            await _authService.RevokeTokenAsync(refreshToken);
            return NoContent();
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Dane zalogowanego użytkownika.</summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public IActionResult Me()
    {
        var user = new UserDto
        {
            Id         = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
            Email      = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            FirstName  = User.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty,
            LastName   = User.FindFirstValue(ClaimTypes.Surname) ?? string.Empty,
            Department = User.FindFirstValue("department") ?? string.Empty,
            Roles      = User.FindAll(ClaimTypes.Role).Select(c => c.Value)
        };

        return Ok(user);
    }
}
