using Microsoft.Extensions.Options;
using Cleanish.Shared.Clock;
using Cleanish.Shared.Helpers;
using System.Security.Claims;
using Cleanish.Shared.Security;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Cleanish.Impl.Shared.Security.WebToken;

internal class WebTokenService : IWebTokenService
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
            SigningCredentials = new SigningCredentials(_tokenSettings.GetSecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _tokenSettings.Issuer,
            Audience = _tokenSettings.Audience
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
                    ValidIssuer = _tokenSettings.Issuer,
                    ValidAudience = _tokenSettings.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
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
