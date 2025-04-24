using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MyApp.Infrastructure.Services;

public interface IJwtTokenService
{
    string GenerateToken(string userId, string username, IEnumerable<string>? roles = null);
    bool ValidateToken(string token);
    ClaimsPrincipal? GetPrincipalFromToken(string token);
}

public class JwtTokenService : IJwtTokenService
{
    private readonly ApplicationSettings _settings;
    private readonly SigningCredentials _signingCredentials;
    private readonly TokenValidationParameters _validationParameters;

    public JwtTokenService(ApplicationSettings settings)
    {
        _settings = settings;
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Jwt.Secret));
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = settings.Jwt.Issuer,
            ValidAudience = settings.Jwt.Audience,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };
    }

    public string GenerateToken(string userId, string username, IEnumerable<string>? roles = null)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.UniqueName, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (roles != null)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var token = new JwtSecurityToken(
            issuer: _settings.Jwt.Issuer,
            audience: _settings.Jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.Jwt.ExpirationInMinutes),
            signingCredentials: _signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, _validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, _validationParameters, out _);
        }
        catch
        {
            return null;
        }
    }
}