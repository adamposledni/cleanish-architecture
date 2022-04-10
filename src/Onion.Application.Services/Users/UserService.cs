using Onion.Application.Services.Auth.Exceptions;
using Onion.Application.Services.Auth.Models;
using Onion.Application.Services.Security;
using Onion.Application.Services.Users.Exceptions;
using Onion.Application.Services.Users.Models;
using Onion.Core.Helpers;
using Onion.Core.Mapper;
using Onion.Core.Security;
using Onion.Core.Exceptions;
using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;

namespace Onion.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IPasswordProvider _passwordProvider;
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IUserRepository _userRepository;

    public UserService(
        IMapper mapper,
        IPasswordProvider passwordProvider,
        ISecurityContextProvider securityContextProvider,
        IGoogleAuthProvider googleAuthProvider, 
        IUserRepository userRepository)
    {
        _mapper = mapper;
        _passwordProvider = passwordProvider;
        _securityContextProvider = securityContextProvider;
        _googleAuthProvider = googleAuthProvider;
        _userRepository = userRepository;
    }

    public async Task<UserRes> GetAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new UserNotFoundException();

        return _mapper.Map<User, UserRes>(user);
    }

    public async Task<IEnumerable<UserRes>> ListAsync()
    {
        var users = await _userRepository.CachedListAsync();
        return _mapper.MapCollection<User, UserRes>(users);
    }

    public async Task<UserRes> CreateAsync(UserReq model)
    {
        Guard.NotNull(model, nameof(model));

        if (await _userRepository.EmailAlreadyExistsAsync(model.Email)) throw new EmailAlreadyTakenException();

        (byte[] hash, byte[] salt) = _passwordProvider.Hash(model.Password);
        var newUser = _mapper.Map<UserReq, User>(model, u =>
        {
            u.PasswordHash = hash;
            u.PasswordSalt = salt;
        });
        var user = await _userRepository.CreateAsync(newUser);

        return _mapper.Map<User, UserRes>(user);
    }

    public async Task<UserRes> GoogleLinkAsync(IdTokenAuthReq model)
    {
        Guard.NotNull(model, nameof(model));

        var securityContext = _securityContextProvider.SecurityContext;
        if (securityContext == null || securityContext.Type != SecurityContextType.User)
            throw new UnauthorizedException();

        var googleIdentity = await _googleAuthProvider.GetIdentityAsync(model.IdToken);
        if (googleIdentity == null) throw new InvalidGoogleIdTokenException();

        var user = await _userRepository.GetByIdAsync(securityContext.SubjectId);
        if (user == null) throw new UserNotFoundException();

        if (!string.IsNullOrWhiteSpace(user.GoogleSubjectId)) throw new GoogleLinkAlreadyExistsException();

        user.GoogleSubjectId = googleIdentity.SubjectId;
        await _userRepository.UpdateAsync(user);

        return _mapper.Map<User, UserRes>(user);
    }
}