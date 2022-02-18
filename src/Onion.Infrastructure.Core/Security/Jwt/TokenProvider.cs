using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Onion.Core.Clock;
using Onion.Core.Helpers;
using Onion.Core.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Onion.Infrastructure.Core.Security.Jwt;

public class TokenProvider : ITokenProvider
{
    private readonly TokenProviderSettings _tokenSettings;
    private readonly IClockProvider _clockProvider;

    public TokenProvider(IOptions<TokenProviderSettings> tokenSettings, IClockProvider clockProvider)
    {
        _tokenSettings = tokenSettings.Value;
        _clockProvider = clockProvider;
    }

    public string GenerateJwt(IEnumerable<Claim> claims, int expiresIn)
    {
        Guard.NotNullOrEmpty(claims, nameof(claims));
        Guard.Min(expiresIn, 1, nameof(expiresIn));

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = _clockProvider.Now.AddMinutes(expiresIn),
            SigningCredentials = new SigningCredentials(_tokenSettings.GetSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool IsTokenValid(string token)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(token, nameof(token));

        JwtSecurityTokenHandler tokenHandler = new();
        try
        {
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _tokenSettings.GetSecurityKey(),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out var validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public string GetRandomToken(int bytes)
    {
        Guard.Min(bytes, 0, nameof(bytes));

        var randomNumber = new byte[bytes];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
