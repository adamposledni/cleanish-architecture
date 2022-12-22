using Mapster;
using MediatR;
using Cleanish.App.Data.Cache;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Logic.Common.Attributes;
using Cleanish.App.Logic.Users.Exceptions;
using Cleanish.App.Logic.Users.Models;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Logic.Common.Security;

namespace Cleanish.App.Logic.Users.UseCases;

[Authorize]
public class GetCurrentUserRequest : IRequest<UserRes> { }

internal class GetCurrentUserRequestHandler : IRequestHandler<GetCurrentUserRequest, UserRes>
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurityContextProvider _securityContextProvider;

    public GetCurrentUserRequestHandler(Cached<IUserRepository> userRepository, ISecurityContextProvider securityContextProvider)
    {
        _userRepository = userRepository.Value;
        _securityContextProvider = securityContextProvider;
    }

    public async Task<UserRes> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        Guid subjectId = _securityContextProvider.GetSubjectId();

        User user = await _userRepository.GetByIdAsync(subjectId);
        if (user == null) throw new UserNotFoundException();

        return user.Adapt<UserRes>();
    }
}