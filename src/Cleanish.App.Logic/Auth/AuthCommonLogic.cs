using Mapster;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Data.Security;
using Cleanish.App.Logic.Auth.Models;
using Cleanish.Shared.Clock;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Cleanish.App.Logic.Common.Mapper;

namespace Cleanish.App.Logic.Auth;

internal static class AuthCommonLogic
{
    public async static Task<AuthRes> IssueAccessAsync(
        IRefreshTokenRepository refreshTokenRepository,
        IWebTokenService tokenProvider,
        ICryptographyService cryptographyService,
        IClockProvider clockProvider,
        User user,
        int accessTokenLifetime,
        int refreshTokenLifetime)
    {
        string accessToken = GenerateAccessToken(tokenProvider, user, accessTokenLifetime);
        var refreshToken = GenerateRefeshToken(tokenProvider, cryptographyService, clockProvider, user, refreshTokenLifetime);

        refreshToken = await refreshTokenRepository.CreateAsync(refreshToken);

        return user.Adapt<AuthRes>(a =>
        {
            a.AccessToken = accessToken;
            a.RefreshToken = refreshToken.Token;
        });
    }

    private static string GenerateAccessToken(IWebTokenService webTokenService, User user, int tokenLifetime)
    {
        List<Claim> claims = new()
        {
            new Claim("sub", user.Id.ToString()),
            new Claim("email", user.Email),
            new Claim("role", user.Role.ToString())
        };
        return webTokenService.CreateWebToken(claims, tokenLifetime);
    }

    private static RefreshToken GenerateRefeshToken(
        IWebTokenService tokenProvider,
        ICryptographyService cryptographyService,
        IClockProvider clockProvider,
        User user,
        int refreshTokenLifetime)
    {
        return new RefreshToken(
            cryptographyService.GetRandomString(32),
            user.Id,
            clockProvider.Now.AddMinutes(refreshTokenLifetime)
        );
    }
}
