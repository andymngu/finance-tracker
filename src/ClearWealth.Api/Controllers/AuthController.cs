// src/ClearWealth.Api/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    public AuthController(IConfiguration config) => _config = config;

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        // TODO: verify against real user in database
        // For now — accept any credentials and issue a token
        if (req.Email != "demo@clearwealth.com" || req.Password != "password")
            return Unauthorized();

        var key = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "clearwealth",
            audience: "clearwealth",
            claims: new[]
            {
                // This claim becomes User.FindFirstValue(ClaimTypes.NameIdentifier)
                new Claim(ClaimTypes.NameIdentifier,
                          "00000000-0000-0000-0000-000000000001"),
                new Claim(ClaimTypes.Email, req.Email),
            },
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public record LoginRequest(string Email, string Password);