using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Onion.Application.DataAccess.Entities;
using Onion.Application.Services.Security;
using Onion.Core.Clock;
using Onion.Core.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Onion.Infrastructure.Security.Jwt
{
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
            JwtSecurityTokenHandler tokenHandler = new();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.JwtSigningKey);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = _clockProvider.Now.AddMinutes(expiresIn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
