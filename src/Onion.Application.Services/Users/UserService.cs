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
using Onion.Core.Cache;

namespace Onion.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IObjectMapper _mapper;
    private readonly IPasswordProvider _passwordProvider;
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly IDatabaseRepositoryManager _databaseRepositoryManager;
    private readonly Func<CacheStrategy, IUserRepository> _userRepositoryFactory;
    private readonly IUserRepository _userRepository;
    private readonly IUserRepository _cachedUserRepository;

    public UserService(
        IObjectMapper mapper,
        IPasswordProvider passwordProvider,
        ISecurityContextProvider securityContextProvider,
        IGoogleAuthProvider googleAuthProvider, 
        IDatabaseRepositoryManager databaseRepositoryManager,
        Func<CacheStrategy, IUserRepository> userRepositoryFactory)
    {
        _mapper = mapper;
        _passwordProvider = passwordProvider;
        _securityContextProvider = securityContextProvider;
        _googleAuthProvider = googleAuthProvider;
        _databaseRepositoryManager = databaseRepositoryManager;
        _userRepositoryFactory = userRepositoryFactory;
        _userRepository = _databaseRepositoryManager.GetRepository<IUserRepository, User>(CacheStrategy.Bypass);
        _cachedUserRepository = _databaseRepositoryManager.GetRepository<IUserRepository, User>(CacheStrategy.Use);
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
        return _mapper.MapCollection<User, UserRes>(users);
    }

    public async Task<UserRes> CreateAsync(UserReq model)
    {
        Guard.NotNull(model, nameof(model));

        if (await _userRepository.AnyWithEmailAsync(model.Email))
        {
            throw new EmailAlreadyTakenException();
        }

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
        Guard.NotNull(user, nameof(user));

        if (!string.IsNullOrWhiteSpace(user.GoogleSubjectId)) throw new GoogleLinkAlreadyExistsException();

        user.GoogleSubjectId = googleIdentity.SubjectId;
        await _userRepository.UpdateAsync(user);

        return _mapper.Map<User, UserRes>(user);
    }

    public async Task<Foo1Res> FooAsync()
    {
        var repo = _userRepositoryFactory(CacheStrategy.Bypass);
        var foo = await repo.FooAsync();
        return _mapper.Map<User, Foo1Res>(foo);
    }
}