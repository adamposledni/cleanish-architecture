using Onion.Application.DataAccess.BaseExceptions;
using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Auth.Exceptions;
using Onion.Application.Services.Auth.Models;
using Onion.Application.Services.Security;
using Onion.Application.Services.UserManagement.Exceptions;
using Onion.Application.Services.UserManagement.Models;
using Onion.Core.Helpers;
using Onion.Core.Mapper;
using Onion.Core.Security;

namespace Onion.Application.Services.UserManagement;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordProvider _passwordProvider;
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly IGoogleAuthProvider _googleAuthProvider;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        IPasswordProvider passwordProvider,
        ISecurityContextProvider securityContextProvider,
        IGoogleAuthProvider googleAuthProvider)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordProvider = passwordProvider;
        _securityContextProvider = securityContextProvider;
        _googleAuthProvider = googleAuthProvider;
    }

    public async Task<UserRes> GetAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new UserNotFoundException();

        return _mapper.Map<User, UserRes>(user);
    }

    public async Task<IEnumerable<UserRes>> ListAsync()
    {
        var users = await _userRepository.ListAsync();
        return users.Select(u => _mapper.Map<User, UserRes>(u)).ToList();
    }

    public async Task<UserRes> CreateAsync(UserReq model)
    {
        Guard.NotNull(model, nameof(model));

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