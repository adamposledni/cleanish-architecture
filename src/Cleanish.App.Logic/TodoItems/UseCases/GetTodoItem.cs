using FluentValidation;
using MediatR;
using Cleanish.App.Data.Cache;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Logic.Common.Attributes;
using Cleanish.App.Logic.Common.Security;
using Cleanish.App.Logic.TodoItems.Exceptions;
using Cleanish.App.Logic.TodoLists.Models;
using Cleanish.App.Data.Database.Entities;

namespace Cleanish.App.Logic.TodoItems.UseCases;

[Authorize]
public class GetTodoItemRequest : IRequest<TodoItemRes>
{
    public Guid TodoItemId { get; set; }

    public GetTodoItemRequest(Guid todoItemId)
    {
        TodoItemId = todoItemId;
    }
}

internal class GetTodoItemRequestValidator : AbstractValidator<GetTodoItemRequest>
{
    public GetTodoItemRequestValidator()
    {
        RuleFor(x => x.TodoItemId).NotEmpty();
    }
}

internal class GetTodoItemRequestHandler : IRequestHandler<GetTodoItemRequest, TodoItemRes>
{
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoItemRepository _cachedTodoItemRepository;

    public GetTodoItemRequestHandler(
        ISecurityContextProvider securityContextProvider,
        Cached<ITodoItemRepository> cachedTodoItemRepository)
    {
        _securityContextProvider = securityContextProvider;
        _cachedTodoItemRepository = cachedTodoItemRepository.Value;
    }

    public async Task<TodoItemRes> Handle(GetTodoItemRequest request, CancellationToken cancellationToken)
    {
        Guid subjectId = _securityContextProvider.GetSubjectId();
        TodoItem todoItem = await _cachedTodoItemRepository.GetByIdAsync(request.TodoItemId);
        if (todoItem == null) throw new TodoItemNotFoundException();
        TodoItemCommonLogic.ValidateTodoItemOwnership(todoItem, subjectId);

        return todoItem.Adapt<TodoItemRes>();
    }
}
