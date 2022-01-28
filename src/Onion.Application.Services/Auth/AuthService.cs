using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Exceptions.Auth;
using Onion.Application.DataAccess.Exceptions.RefreshToken;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Auth.Models;
using Onion.Application.Services.Security;
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

    public AuthService(
        IUserRepository userRepository,
        ITokenProvider tokenProvider,
        IGoogleAuthProvider googleAuthProvider,
        IMapper mapper,
        IPasswordProvider passwordProvider,
        ISecurityContextProvider contextProvider,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
        _googleAuthProvider = googleAuthProvider;
        _mapper = mapper;
        _passwordProvider = passwordProvider;
        _securityContextProvider = contextProvider;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthRes> LoginAsync(PasswordAuthReq model)
    {
        var user = await _userRepository.GetByEmailAsync(model.Email);
        if (user == null || !_passwordProvider.Verify(model.Password, user.PasswordHash, user.PasswordSalt))
            throw new InvalidEmailPasswordException();

        return await IssueAccessAsync(user);
    }

    public async Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model)
    {
        var googleIdentity = await _googleAuthProvider.GetIdentityAsync(model.IdToken);
        if (googleIdentity == null) throw new InvalidGoogleIdTokenException();

        var user = await _userRepository.GetByGoogleIdAsync(googleIdentity.SubjectId);
        if (user == null) throw new GoogleLinkMissingException();

        return await IssueAccessAsync(user);
    }

    public async Task<RefreshTokenRes> RevokeRefreshTokenAsync(string refreshToken)
    {
        if (!_tokenProvider.IsTokenValid(refreshToken)) throw new InvalidRefreshTokenException();

        var securityContext = _securityContextProvider.SecurityContext;
        if (securityContext == null || securityContext.Type != SecurityContextType.User)
            throw new UnauthorizedException();

        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAndUserIdAsync(
            refreshToken, securityContext.SubjectId);

        refreshTokenEntity = await RevokeRefreshTokenAsync(refreshTokenEntity);

        return _mapper.Map<RefreshToken, RefreshTokenRes>(refreshTokenEntity);
    }

    public async Task<AuthRes> RefreshAccessTokenAsync(string refreshToken)
    {
        if (!_tokenProvider.IsTokenValid(refreshToken)) throw new InvalidRefreshTokenException();

        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (refreshTokenEntity.IsRevoked) throw new RefreshTokenAlreadyRevokedException();

        refreshTokenEntity = await RevokeRefreshTokenAsync(refreshTokenEntity);

        var user = refreshTokenEntity.User;
        return await IssueAccessAsync(user);
    }

    private async Task<RefreshToken> RevokeRefreshTokenAsync(RefreshToken refreshToken)
    {
        if (refreshToken == null) throw new RefreshTokenNotFoundException();

        refreshToken.IsRevoked = true;
        return await _refreshTokenRepository.UpdateAsync(refreshToken);
    }

    private async Task<AuthRes> IssueAccessAsync(User user)
    {
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
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };
        return _tokenProvider.GenerateJwt(claims, 1);
    }

    private RefreshToken GenerateRefeshToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };
        int expiration = 2; // 7 days
        return new RefreshToken(
            _tokenProvider.GenerateJwt(claims, expiration),
            user.Id);
    }
}
