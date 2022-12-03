using MediatR;
using Cleanish.App.Data.Cache;
using Cleanish.App.Data.Database.Entities.Fields;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Logic.Common.Attributes;
using Cleanish.App.Logic.Users.Models;
using System.Linq;
using System.Threading;

namespace Cleanish.App.Logic.Users.UseCases;

[Authorize(Roles = new[] { UserRole.Admin })]
public class ListUsersRequest : IRequest<IEnumerable<UserRes>>
{
}

internal class ListUsersRequestHandler : IRequestHandler<ListUsersRequest, IEnumerable<UserRes>>
{
    private readonly IUserRepository _userRepository;

    public ListUsersRequestHandler(Cached<IUserRepository> userRepository)
    {
        _userRepository = userRepository.Value;
    }

    public async Task<IEnumerable<UserRes>> Handle(ListUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.ListAsync();
        return users.Select(u => u.Adapt<UserRes>());
    }
}
