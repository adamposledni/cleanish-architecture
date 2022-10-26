using FluentValidation;
using MediatR;
using Onion.App.Data.Cache;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Common.Security;
using Onion.App.Logic.TodoItems.Exceptions;
using Onion.App.Logic.TodoLists.Models;
using System.Threading;

namespace Onion.App.Logic.TodoItems.UseCases;

[Authorize]
public class GetTodoItemRequest : IRequest<TodoItemRes>
{
    public Guid TodoListId { get; set; }
    public Guid TodoItemId { get; set; }

    public GetTodoItemRequest(Guid todoListId, Guid todoItemId)
    {
        TodoListId = todoListId;
        TodoItemId = todoItemId;
    }
}

internal class GetTodoItemRequestValidator : AbstractValidator<GetTodoItemRequest>
{
    public GetTodoItemRequestValidator()
    {
        RuleFor(x => x.TodoListId).NotEmpty();
        RuleFor(x => x.TodoItemId).NotEmpty();
    }
}

internal class GetTodoItemRequestHandler : IRequestHandler<GetTodoItemRequest, TodoItemRes>
{
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoListRepository _todoListRepository;
    private readonly ITodoItemRepository _cachedTodoItemRepository;

    public GetTodoItemRequestHandler(
        ISecurityContextProvider securityContextProvider,
        ITodoListRepository todoListRepository,
        Cached<ITodoItemRepository> cachedTodoItemRepository)
    {
        _securityContextProvider = securityContextProvider;
        _todoListRepository = todoListRepository;
        _cachedTodoItemRepository = cachedTodoItemRepository.Value;
    }

    public async Task<TodoItemRes> Handle(GetTodoItemRequest request, CancellationToken cancellationToken)
    {
        var subjectId = _securityContextProvider.GetSubjectId();
        await TodoItemCommonLogic.ValidateTodoListOwnership(_todoListRepository, request.TodoListId, subjectId);

        var todoItem = await _cachedTodoItemRepository.GetByIdAsync(request.TodoItemId);
        if (todoItem == null) throw new TodoItemNotFoundException();

        return todoItem.Adapt<TodoItemRes>();
    }
}
