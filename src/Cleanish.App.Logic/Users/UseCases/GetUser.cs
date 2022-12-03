using FluentValidation;
using Mapster;
using MediatR;
using Cleanish.App.Data.Cache;
using Cleanish.App.Data.Database.Entities.Fields;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Logic.Common.Attributes;
using Cleanish.App.Logic.Users.Exceptions;
using Cleanish.App.Logic.Users.Models;

namespace Cleanish.App.Logic.Users.UseCases;

[Authorize(Roles = new[] { UserRole.Admin })]
public class GetUserRequest : IRequest<UserRes>
{
    public Guid Id { get; set; }

    public GetUserRequest(Guid userId)
    {
        Id = userId;
    }
}

internal class GetUserRequestValidator : AbstractValidator<GetUserRequest>
{
    public GetUserRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
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
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null) throw new UserNotFoundException();

        return user.Adapt<UserRes>();
    }
}
