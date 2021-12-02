using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Auth.Models;
using Onion.Application.Services.Exceptions;
using Onion.Core.Mapper;
using Onion.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IJwtProvider _jwtService;
        private readonly IGoogleAuthProvider _googleAuthProvider;
        private readonly IMapper _mapper;
        private readonly IPasswordProvider _passwordProvider;

        public AuthService(
            IRepositoryManager repositoryManager,
            IJwtProvider jwtService,
            IGoogleAuthProvider googleAuthProvider,
            IMapper mapper,
            IPasswordProvider passwordProvider)
        {
            _repositoryManager = repositoryManager;
            _jwtService = jwtService;
            _googleAuthProvider = googleAuthProvider;
            _mapper = mapper;
            _passwordProvider = passwordProvider;
        }

        public async Task<AuthRes> LoginAsync(PasswordAuthReq model)
        {
            var user = await _repositoryManager.UserRepository.GetByEmailAsync(model.Email);
            if (user == null || !_passwordProvider.Verify(model.Password, user.PasswordHash, user.PasswordSalt))
                throw new InvalidEmailPasswordException();

            return _mapper.Map<User, AuthRes>(user, a => a.AccessToken = GenerateJwt(user));
        }

        public async Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model)
        {
            var googleIdentity = await _googleAuthProvider.GetIdentityAsync(model.IdToken);
            if (googleIdentity == null) throw new InvalidGoogleIdTokenException();

            var user = await _repositoryManager.UserRepository.GetByEmailAsync(googleIdentity.Email);
            // new user
            if (user == null)
            {
                user = new User()
                {
                    Email = googleIdentity.Email,
                    GoogleSubjectId = googleIdentity.SubjectId
                };
                await _repositoryManager.UserRepository.CreateAsync(user);
            }
            // already existing without Google subject id
            else if (string.IsNullOrWhiteSpace(user.GoogleSubjectId))
            {
                user.GoogleSubjectId = googleIdentity.SubjectId;
            }

            return _mapper.Map<User, AuthRes>(user, a => a.AccessToken = GenerateJwt(user));
        }

        public Task<AuthRes> FacebookLoginAsync(IdTokenAuthReq model)
        {
            throw new NotImplementedException();
        }

        private string GenerateJwt(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Upn, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };
            return _jwtService.GenerateJwt(claims);
        }
    }
}
