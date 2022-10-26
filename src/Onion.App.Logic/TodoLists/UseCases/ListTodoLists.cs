using MediatR;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Common.Security;
using Onion.App.Logic.TodoLists.Models;
using System.Linq;
using System.Threading;

namespace Onion.App.Logic.TodoLists.UseCases;

[Authorize]
public class ListTodoListsRequest : IRequest<IEnumerable<TodoListBriefRes>>
{
}

internal class ListTodoListsRequestHandler : IRequestHandler<ListTodoListsRequest, IEnumerable<TodoListBriefRes>>
{
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoListRepository _cachedTodoListRepository;

    public ListTodoListsRequestHandler(
        ISecurityContextProvider securityContextProvider,
        Cached<ITodoListRepository> cachedTodoListRepository)
    {
        _securityContextProvider = securityContextProvider;
        _cachedTodoListRepository = cachedTodoListRepository.Value;
    }

    public async Task<IEnumerable<TodoListBriefRes>> Handle(ListTodoListsRequest request, CancellationToken cancellationToken)
    {
        var subjectId = _securityContextProvider.GetSubjectId();
        var todoLists = await _cachedTodoListRepository.ListAsync(subjectId);
        return todoLists.Select(tl => tl.Adapt<TodoListBriefRes>());
    }
}
