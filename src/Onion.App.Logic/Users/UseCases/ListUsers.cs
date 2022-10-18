using MediatR;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Users.Models;
using Onion.Shared.Mapper;
using System.Threading;

namespace Onion.App.Logic.Users.UseCases;

[Authorize]
public class ListUsersRequest : IRequest<IEnumerable<UserRes>>
{
}

internal class ListUsersRequestHandler : IRequestHandler<ListUsersRequest, IEnumerable<UserRes>>
{
    private readonly IUserRepository _userRepository;
    private readonly IObjectMapper _mapper;

    public ListUsersRequestHandler(Cached<IUserRepository> userRepository, IObjectMapper mapper)
    {
        _userRepository = userRepository.Value;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserRes>> Handle(ListUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.ListAsync();
        return _mapper.MapCollection<UserRes>(users);
    }
}
