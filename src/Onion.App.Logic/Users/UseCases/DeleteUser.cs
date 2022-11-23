using FluentValidation;
using MediatR;
using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Common.Security;
using Onion.App.Logic.Users.Exceptions;
using Onion.App.Logic.Users.Models;
using Onion.Shared.Exceptions;

namespace Onion.App.Logic.Users.UseCases;

public class DeleteUserRequest : IRequest<UserRes>
{
    public Guid Id { get; init; }

    public DeleteUserRequest(Guid userId)
    {
        Id = userId;
    }
}

internal class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

internal class DeleteUserRequestHandler : IRequestHandler<DeleteUserRequest, UserRes>
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurityContextProvider _securityContextProvider;

    public DeleteUserRequestHandler(
        IUserRepository userRepository,
        ISecurityContextProvider securityContextProvider)
    {
        _userRepository = userRepository;
        _securityContextProvider = securityContextProvider;
    }

    public async Task<UserRes> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        if (user.Id != _securityContextProvider.GetSubjectId())
        {
            throw new NotAuthorizedException();
        }
        user = await _userRepository.DeleteAsync(user);
        return user.Adapt<UserRes>();
    }
}
