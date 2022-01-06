using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Exceptions.Auth;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Auth.Models;
using Onion.Application.Services.Security;
using Onion.Core.Mapper;
using Onion.Core.Security;
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

            var jwt = _jwtService.GenerateJwt(user);
            return _mapper.Map<User, AuthRes>(user, a => a.AccessToken = jwt);
        }

        public async Task<AuthRes> GoogleLoginAsync(IdTokenAuthReq model)
        {
            var googleIdentity = await _googleAuthProvider.GetIdentityAsync(model.IdToken);
            if (googleIdentity == null) throw new InvalidGoogleIdTokenException();

            var user = await _repositoryManager.UserRepository.GetByGoogleIdAsync(googleIdentity.SubjectId);
            if (user == null) throw new GoogleLinkMissingException();

            var jwt = _jwtService.GenerateJwt(user);
            return _mapper.Map<User, AuthRes>(user, a => a.AccessToken = jwt);
        }
    }
}
