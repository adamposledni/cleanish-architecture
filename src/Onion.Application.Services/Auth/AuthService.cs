using Microsoft.Extensions.Options;
using Onion.Application.DataAccess.BaseExceptions;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Auth.Exceptions;
using Onion.Application.Services.Auth.Models;
using Onion.Application.Services.Common;
using Onion.Application.Services.Security;
using Onion.Core.Clock;
using Onion.Core.Helpers;
using Onion.Core.Mapper;
using Onion.Core.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Onion.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IMapper _mapper;
    private readonly IPasswordProvider _passwordProvider;
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ApplicationSettings _applicationSettings;
    private readonly IClockProvider _clockProvider;

    public AuthService(
        IUserRepository userRepository,
        ITokenProvider tokenProvider,
        IGoogleAuthProvider googleAuthProvider,
        IMapper mapper,
        IPasswordProvider passwordProvider,
        ISecurityContextProvider contextProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<ApplicationSettings> applicationSettings, 
        IClockProvider clockProvider)
    {
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
        _googleAuthProvider = googleAuthProvider;
        _mapper = mapper;
        _passwordProvider = passwordProvider;
        _securityContextProvider = contextProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _applicationSettings = applicationSettings.Value;
        _clockProvider = clockProvider;
    }

    public async Task<AuthRes> LoginAsync(PasswordAuthReq model)
    {
        Guard.NotNull(model, nameof(model));

        var user = await _userRepository.GetByEmailAsync(model.Email);
        if (user == null || !_passwordProvider.Verify(model.Password, user.PasswordHash, user.PasswordSalt))
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
        if (securityContext == null || securityContext.Type != SecurityContextType.User)
            throw new UnauthorizedException();
        
        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(model.RefreshToken);

        if (refreshTokenEntity == null) throw new RefreshTokenNotFoundException();
        if (refreshTokenEntity.IsExpired(_clockProvider.Now)) throw new InvalidRefreshTokenException();

        refreshTokenEntity = await RevokeRefreshTokenAsync(refreshTokenEntity);
        return _mapper.Map<RefreshToken, RefreshTokenRes>(refreshTokenEntity);
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

        await _refreshTokenRepository.CreateAsync(refreshToken);

        return _mapper.Map<User, AuthRes>(
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
        return _tokenProvider.GenerateJwt(claims, _applicationSettings.AccessTokenLifetime);
    }

    private RefreshToken GenerateRefeshToken(User user)
    {
        Guard.NotNull(user, nameof(user));

        return new RefreshToken(
            _tokenProvider.GetRandomToken(32),
            user.Id,
            _clockProvider.Now.AddMinutes(_applicationSettings.RefreshTokenLifetime)
        );
    }
}
