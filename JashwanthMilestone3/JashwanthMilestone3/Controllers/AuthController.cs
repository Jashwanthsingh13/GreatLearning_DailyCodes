using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JashwanthMilestone3.Models;
using JashwanthMilestone3.Services;

namespace JashwanthMilestone3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDataStore store, IConfiguration config) : ControllerBase
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (store.Users.Any(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)))
        {
            return BadRequest(new { message = "Username already exists." });
        }

        var user = new User
        {
            Id = store.NextUserId(),
            Username = request.Username.Trim()
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        store.Users.Add(user);

        return Ok(new { message = "User registered successfully. Please log in." });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var user = store.Users.FirstOrDefault(u =>
            u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase));

        if (user is null)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verifyResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        var expiresInSeconds = 3600;
        var expiresAt = DateTime.UtcNow.AddSeconds(expiresInSeconds);
        var token = CreateToken(user, expiresAt);

        return Ok(new
        {
            token,
            expires_in = expiresInSeconds,
            user = new { username = user.Username }
        });
    }

    private string CreateToken(User user, DateTime expiresAt)
    {
        var key = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing.");
        var issuer = config["Jwt:Issuer"];
        var audience = config["Jwt:Audience"];

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
