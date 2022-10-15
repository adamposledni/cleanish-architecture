using Microsoft.Extensions.Options;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;
using Onion.App.Data.Security;
using Onion.App.Logic.Auth.Exceptions;
using Onion.App.Logic.Auth.Models;
using Onion.App.Logic.Common;
using Onion.App.Logic.Security;
using Onion.Shared.Clock;
using Onion.Shared.Helpers;
using Onion.Shared.Mapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Onion.App.Logic.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IWebTokenService _webTokenService;
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IObjectMapper _mapper;
    private readonly ICryptographyService _cryptographyService;
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ApplicationSettings _applicationSettings;
    private readonly IClockProvider _clockProvider;

    public AuthService(
        IWebTokenService tokenProvider,
        IGoogleAuthProvider googleAuthProvider,
        IObjectMapper mapper,
        ICryptographyService passwordProvider,
        ISecurityContextProvider contextProvider,
        IOptions<ApplicationSettings> applicationSettings,
        IClockProvider clockProvider,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _webTokenService = tokenProvider;
        _googleAuthProvider = googleAuthProvider;
        _mapper = mapper;
        _cryptographyService = passwordProvider;
        _securityContextProvider = contextProvider;
        _applicationSettings = applicationSettings.Value;
        _clockProvider = clockProvider;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthRes> LoginAsync(PasswordAuthReq model)
    {
        Guard.NotNull(model, nameof(model));

        var user = await _userRepository.GetByEmailAsync(model.Email);
        if (user == null || !_cryptographyService.VerifyStringHash(model.Password, user.PasswordHash, user.PasswordSalt))
            throw new InvalidEmailPasswordException();

        return await IssueAccessAsync(user);
    }

    public async Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model)
    {
        Guard.NotNull(model, nameof(model));

        var googleIdentity = await _googleAuthProvider.GetIdentityAsync(model.IdToken);
        if (googleIdentity == null) throw new InvalidGoogleIdTokenException();

        var user = await _userRepository.GetByGoogleIdAsync(googleIdentity.SubjectId);
        if (user == null) throw new GoogleLinkMissingException();

        return await IssueAccessAsync(user);
    }

    public async Task<RefreshTokenRes> RevokeRefreshTokenAsync(RefreshTokenReq model)
    {
        Guard.NotNull(model, nameof(model));

        var securityContext = _securityContextProvider.SecurityContext;
        Guard.NotNull(securityContext, nameof(securityContext));

        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAndUserIdAsync(model.RefreshToken, securityContext.SubjectId);
        if (refreshTokenEntity == null) throw new RefreshTokenNotFoundException();
        if (refreshTokenEntity.IsExpired(_clockProvider.Now)) throw new InvalidRefreshTokenException();

        refreshTokenEntity = await RevokeRefreshTokenAsync(refreshTokenEntity);
        return _mapper.Map<RefreshTokenRes>(refreshTokenEntity);
    }

    public async Task<AuthRes> RefreshAccessTokenAsync(RefreshTokenReq model)
    {
        Guard.NotNull(model, nameof(model));

        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(model.RefreshToken);
        if (refreshTokenEntity == null) throw new RefreshTokenNotFoundException();
        if (refreshTokenEntity.IsExpired(_clockProvider.Now)) throw new InvalidRefreshTokenException();
        if (refreshTokenEntity.IsRevoked) throw new RefreshTokenAlreadyRevokedException();

        return await IssueAccessAsync(refreshTokenEntity.User);
    }

    private async Task<RefreshToken> RevokeRefreshTokenAsync(RefreshToken refreshToken)
    {
        Guard.NotNull(refreshToken, nameof(refreshToken));

        if (refreshToken.IsRevoked) throw new RefreshTokenAlreadyRevokedException();
        refreshToken.IsRevoked = true;
        return await _refreshTokenRepository.UpdateAsync(refreshToken);
    }

    private async Task<AuthRes> IssueAccessAsync(User user)
    {
        Guard.NotNull(user, nameof(user));

        string accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefeshToken(user);

        refreshToken = await _refreshTokenRepository.CreateAsync(refreshToken);

        return _mapper.Map<AuthRes>(
            user,
            a =>
            {
                a.AccessToken = accessToken;
                a.RefreshToken = refreshToken.Token;
            });
    }

    private string GenerateAccessToken(User user)
    {
        Guard.NotNull(user, nameof(user));

        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        return _webTokenService.CreateWebToken(claims, _applicationSettings.AccessTokenLifetime);
    }

    private RefreshToken GenerateRefeshToken(User user)
    {
        Guard.NotNull(user, nameof(user));

        return new RefreshToken(
            _cryptographyService.GetRandomString(32),
            user.Id,
            _clockProvider.Now.AddMinutes(_applicationSettings.RefreshTokenLifetime)
        );
    }
}
