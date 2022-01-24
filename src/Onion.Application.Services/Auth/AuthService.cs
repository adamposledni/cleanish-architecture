using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Exceptions.Auth;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Auth.Models;
using Onion.Application.Services.Security;
using Onion.Application.Services.Security.Models;
using Onion.Core.Mapper;
using Onion.Core.Security;
using System.Threading.Tasks;
using System;
using Onion.Application.DataAccess.Exceptions.User;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Onion.Core.Clock;

namespace Onion.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly IGoogleAuthProvider _googleAuthProvider;
        private readonly IMapper _mapper;
        private readonly IPasswordProvider _passwordProvider;
        private readonly ISecurityContextProvider _securityContextProvider;
        private readonly IClockProvider _clockProvider;

        public AuthService(
            IRepositoryManager repositoryManager,
            ITokenProvider tokenProvider,
            IGoogleAuthProvider googleAuthProvider,
            IMapper mapper,
            IPasswordProvider passwordProvider, 
            ISecurityContextProvider contextProvider,
            IClockProvider clockProvider)
        {
            _repositoryManager = repositoryManager;
            _tokenProvider = tokenProvider;
            _googleAuthProvider = googleAuthProvider;
            _mapper = mapper;
            _passwordProvider = passwordProvider;
            _securityContextProvider = contextProvider;
            _clockProvider = clockProvider;
        }

        public async Task<AuthRes> LoginAsync(PasswordAuthReq model)
        {
            var user = await _repositoryManager.UserRepository.GetByEmailAsync(model.Email);
            if (user == null || !_passwordProvider.Verify(model.Password, user.PasswordHash, user.PasswordSalt))
                throw new InvalidEmailPasswordException();

            return IssueAccess(user);
        }

        public async Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model)
        {
            var googleIdentity = await _googleAuthProvider.GetIdentityAsync(model.IdToken);
            if (googleIdentity == null) throw new InvalidGoogleIdTokenException();

            var user = await _repositoryManager.UserRepository.GetByGoogleIdAsync(googleIdentity.SubjectId);
            if (user == null) throw new GoogleLinkMissingException();

            return IssueAccess(user);
        }

        public async Task<bool> RevokeTokenAsync(object model)
        {
            return true;
        }

        public async Task<bool> RefreshTokenAsync(object model)
        {
            return true;
        }

        private AuthRes IssueAccess(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefeshToken(user);
            // TODO: save refresh token

            return _mapper.Map<User, AuthRes>(
                user,
                a => {
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
            return _tokenProvider.GenerateJwt(claims, 60);
        }

        private RefreshToken GenerateRefeshToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };
            int expiration = 10080; // 7 days
            return new RefreshToken(
                _tokenProvider.GenerateJwt(claims, expiration),
                user.Id);
        }
    }
}
