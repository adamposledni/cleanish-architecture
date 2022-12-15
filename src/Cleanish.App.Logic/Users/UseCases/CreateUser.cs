using FluentValidation;
using Mapster;
using MediatR;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Data.Security;
using Cleanish.App.Logic.Users.Exceptions;
using Cleanish.App.Logic.Users.Models;

namespace Cleanish.App.Logic.Users.UseCases;

public class CreateUserRequest : IRequest<UserRes>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

internal class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}

internal class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, UserRes>
{
    private readonly IUserRepository _userRepository;
    private readonly ICryptographyService _cryptographyService;

    public CreateUserRequestHandler(IUserRepository userRepository, ICryptographyService passwordProvider)
    {
        _userRepository = userRepository;
        _cryptographyService = passwordProvider;
    }

    public async Task<UserRes> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (await _userRepository.AnyWithEmailAsync(request.Email))
        {
            throw new EmailAlreadyTakenException();
        }

        (byte[] hash, byte[] salt) = _cryptographyService.GetStringHash(request.Password);
        User newUser = request.Adapt<User>(u =>
        {
            u.PasswordHash = hash;
            u.PasswordSalt = salt;
        });
       
        newUser = await _userRepository.CreateAsync(newUser);

        return newUser.Adapt<UserRes>();
    }
}
