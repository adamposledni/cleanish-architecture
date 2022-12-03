using FluentValidation;
using MediatR;
using Cleanish.App.Data.Cache;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Logic.Common.Attributes;
using Cleanish.App.Logic.Common.Security;
using Cleanish.App.Logic.TodoLists.Models;
using System.Linq;

namespace Cleanish.App.Logic.TodoItems.UseCases;

[Authorize]
public class ListTodoItemsRequest : IRequest<IEnumerable<TodoItemRes>>
{
}

internal class ListTodoItemsRequestHandler : IRequestHandler<ListTodoItemsRequest, IEnumerable<TodoItemRes>>
{
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoItemRepository _cachedTodoItemRepository;

    public ListTodoItemsRequestHandler(
        ISecurityContextProvider securityContextProvider,
        Cached<ITodoItemRepository> cachedTodoItemRepository)
    {
        _securityContextProvider = securityContextProvider;
        _cachedTodoItemRepository = cachedTodoItemRepository.Value;
    }

    public async Task<IEnumerable<TodoItemRes>> Handle(ListTodoItemsRequest request, CancellationToken cancellationToken)
    {
        var subjectId = _securityContextProvider.GetSubjectId();

        var todoItems = await _cachedTodoItemRepository.ListByUserIdAsync(subjectId);
        return todoItems.Select(t => t.Adapt<TodoItemRes>());
    }
}
