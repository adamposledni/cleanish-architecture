using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Onion.App.Data.Security;
using Onion.Shared.Clock;
using Onion.Shared.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Onion.Impl.App.Data.Security.WebToken;

public class WebTokenService : IWebTokenService
{
    private readonly WebTokenSettings _tokenSettings;
    private readonly IClockProvider _clockProvider;

    public WebTokenService(IOptions<WebTokenSettings> tokenSettings, IClockProvider clockProvider)
    {
        _tokenSettings = tokenSettings.Value;
        _clockProvider = clockProvider;
    }

    public string CreateWebToken(IEnumerable<Claim> claims, int expiresIn)
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

    public bool IsWebTokenValid(string token)
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
                out _);
        }
        catch
        {
            return false;
        }
        return true;
    }
}
