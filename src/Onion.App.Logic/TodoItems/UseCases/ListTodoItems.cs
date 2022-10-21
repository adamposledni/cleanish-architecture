using FluentValidation;
using MediatR;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Common.Security;
using Onion.App.Logic.TodoLists.Models;
using Onion.Shared.Mapper;
using System.Threading;

namespace Onion.App.Logic.TodoItems.UseCases;

[Authorize]
public class ListTodoItemsRequest : IRequest<IEnumerable<TodoItemRes>>
{
    public Guid TodoListId { get; set; }

    public ListTodoItemsRequest(Guid todoListId)
    {
        TodoListId = todoListId;
    }
}

internal class ListTodoItemsRequestValidator : AbstractValidator<ListTodoItemsRequest>
{
    public ListTodoItemsRequestValidator()
    {
        RuleFor(x => x.TodoListId).NotEmpty();
    }
}

public class ListTodoItemsRequestHandler : IRequestHandler<ListTodoItemsRequest, IEnumerable<TodoItemRes>>
{
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoListRepository _todoListRepository;
    private readonly IObjectMapper _mapper;
    private readonly ITodoItemRepository _cachedTodoItemRepository;

    public ListTodoItemsRequestHandler(
        ISecurityContextProvider securityContextProvider,
        Cached<ITodoItemRepository> cachedTodoItemRepository,
        ITodoListRepository todoListRepository,
        IObjectMapper mapper)
    {
        _securityContextProvider = securityContextProvider;
        _todoListRepository = todoListRepository;
        _mapper = mapper;
        _cachedTodoItemRepository = cachedTodoItemRepository.Value;
    }

    public async Task<IEnumerable<TodoItemRes>> Handle(ListTodoItemsRequest request, CancellationToken cancellationToken)
    {
        var subjectId = _securityContextProvider.GetSubjectId();
        await TodoItemCommonLogic.ValidateTodoListOwnership(_todoListRepository, request.TodoListId, subjectId);

        var todoItems = await _cachedTodoItemRepository.ListAsync(request.TodoListId);
        return _mapper.MapCollection<TodoItemRes>(todoItems);
    }
}
