using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Abstractions;
using Onion.Application.Services.Exceptions;
using Onion.Application.Services.Models.Auth;
using Onion.Core.Mapper;
using Onion.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(IRepositoryManager repositoryManager, IJwtService jwtService, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<AuthRes> LoginAsync(PasswordAuthReq model)
        {
            var user = await _repositoryManager.UserRepository.GetByEmailAsync(model.Email);
            if (user == null || !Password.Verify(model.Password, user.PasswordHash, user.PasswordSalt)) 
                throw new InvalidEmailPasswordException();

            return _mapper.Map<User, AuthRes>(user, a => a.AccessToken = GenerateJwt(user));
        }

        public Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model)
        {
            throw new NotImplementedException();
        }

        public Task<AuthRes> FacebookLoginAsync(IdTokenAuthReq model)
        {
            throw new NotImplementedException();
        }

        private string GenerateJwt(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
            };
            return _jwtService.GenerateJwt(claims);
        }
    }
}
