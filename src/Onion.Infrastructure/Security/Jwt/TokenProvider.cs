using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Onion.Core.Clock;
using Onion.Core.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Onion.Infrastructure.Security.Jwt;

public class TokenProvider : ITokenProvider
{
    private readonly TokenProviderSettings _tokenSettings;
    private readonly IClockProvider _clockProvider;

    public TokenProvider(IConfiguration configuration, IClockProvider clockProvider)
    {
        _tokenSettings = configuration.GetTokenProviderSettings();
        _clockProvider = clockProvider;
    }

    public string GenerateJwt(IEnumerable<Claim> claims, int expiresIn)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = _clockProvider.Now.AddMinutes(expiresIn),
            SigningCredentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool IsTokenValid(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        try
        {
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    IssuerSigningKey = GetSecurityKey(),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }

    private SecurityKey GetSecurityKey()
    {
        var key = Encoding.ASCII.GetBytes(_tokenSettings?.SigningKey);
        return new SymmetricSecurityKey(key);
    }
}
