using FluentValidation;
using Mapster;
using MediatR;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Entities.Fields;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Users.Exceptions;
using Onion.App.Logic.Users.Models;

namespace Onion.App.Logic.Users.UseCases;

[Authorize(Roles = new[] { UserRole.Admin })]
public class GetUserRequest : IRequest<UserRes>
{
    public Guid UserId { get; set; }

    public GetUserRequest(Guid userId)
    {
        UserId = userId;
    }
}

internal class GetUserRequestValidator : AbstractValidator<GetUserRequest>
{
    public GetUserRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

internal class GetUserRequestHandler : IRequestHandler<GetUserRequest, UserRes>
{
    private readonly IUserRepository _userRepository;

    public GetUserRequestHandler(Cached<IUserRepository> userRepository)
    {
        _userRepository = userRepository.Value;
    }

    public async Task<UserRes> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        return user.Adapt<UserRes>();
    }
}
