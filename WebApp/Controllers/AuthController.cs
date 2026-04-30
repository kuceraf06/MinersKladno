using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Miners.Web.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration config) : ControllerBase
{
    [HttpPost("google")]
    public async Task<IActionResult> Google([FromBody] GoogleLoginRequest request)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(request.Token, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [config["Auth:GoogleClientId"]]
            });
        }
        catch
        {
            return Unauthorized("Neplatný Google token.");
        }

        var allowedEmails = config.GetSection("Auth:AllowedEmails").Get<string[]>() ?? [];
        if (!allowedEmails.Contains(payload.Email, StringComparer.OrdinalIgnoreCase))
            return Forbid();

        var jwt = CreateJwt(payload);
        return Ok(new
        {
            token = jwt,
            user = new { email = payload.Email, name = payload.Name, picture = payload.Picture }
        });
    }

    private string CreateJwt(GoogleJsonWebSignature.Payload payload)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Auth:JwtSecret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, payload.Email),
            new Claim(ClaimTypes.Name, payload.Name),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record GoogleLoginRequest(string Token);
