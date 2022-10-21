using FluentValidation;
using MediatR;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;
using Onion.App.Data.Security;
using Onion.App.Logic.Users.Exceptions;
using Onion.App.Logic.Users.Models;
using Onion.Shared.Mapper;
using System.Threading;

namespace Onion.App.Logic.Users.UseCases;

public class CreateUserRequest : IRequest<UserRes>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

internal class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, UserRes>
{
    private readonly IUserRepository _userRepository;
    private readonly ICryptographyService _cryptographyService;
    private readonly IObjectMapper _mapper;

    public CreateUserRequestHandler(IUserRepository userRepository, ICryptographyService passwordProvider, IObjectMapper mapper)
    {
        _userRepository = userRepository;
        _cryptographyService = passwordProvider;
        _mapper = mapper;
    }

    public async Task<UserRes> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (await _userRepository.AnyWithEmailAsync(request.Email))
        {
            throw new EmailAlreadyTakenException();
        }

        (byte[] hash, byte[] salt) = _cryptographyService.GetStringHash(request.Password);
        var newUser = _mapper.Map<User>(request, u =>
        {
            u.PasswordHash = hash;
            u.PasswordSalt = salt;
        });
        var user = await _userRepository.CreateAsync(newUser);

        return _mapper.Map<UserRes>(user);
    }
}
