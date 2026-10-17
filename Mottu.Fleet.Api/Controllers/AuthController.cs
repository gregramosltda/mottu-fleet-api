using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mottu.Fleet.Api.Dtos;
using Mottu.Fleet.Api.Models;
using Mottu.Fleet.Api.Services;

namespace Mottu.Fleet.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;

    // Usuário hard-coded (em produção, viria do banco)
    private static readonly List<User> Users = new()
    {
        new User { Username = "admin", Password = "admin123", Role = "Admin" },
        new User { Username = "operador", Password = "op123", Role = "User" }
    };

    public AuthController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = Users.FirstOrDefault(u =>
            u.Username == request.Username && u.Password == request.Password);

        if (user is null)
            return Unauthorized(new { erro = "Usuário ou senha inválidos" });

        var token = _jwtService.GenerateToken(user);

        return Ok(new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        });
    }
}
