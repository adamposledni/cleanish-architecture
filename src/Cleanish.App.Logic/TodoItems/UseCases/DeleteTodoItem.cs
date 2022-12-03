using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Logic.Common.Attributes;
using Cleanish.App.Logic.Common.Security;
using Cleanish.App.Logic.TodoItems.Exceptions;
using Cleanish.App.Logic.TodoLists.Models;
using FluentValidation;
using MediatR;

namespace Cleanish.App.Logic.TodoItems.UseCases;

[Authorize]
public class DeleteTodoItemRequest : IRequest<TodoItemRes>
{
    public Guid Id { get; init; }

    public DeleteTodoItemRequest(Guid todoItemId)
    {
        Id = todoItemId;
    }
}

internal class DeleteTodoItemRequestValidator : AbstractValidator<DeleteTodoItemRequest>
{
    public DeleteTodoItemRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

internal class DeleteTodoItemRequestHandler : IRequestHandler<DeleteTodoItemRequest, TodoItemRes>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ISecurityContextProvider _securityContextProvider;

    public DeleteTodoItemRequestHandler(
        ITodoItemRepository todoItemRepository,
        ISecurityContextProvider securityContextProvider)
    {
        _todoItemRepository = todoItemRepository;
        _securityContextProvider = securityContextProvider;
    }

    public async Task<TodoItemRes> Handle(DeleteTodoItemRequest request, CancellationToken cancellationToken)
    {
        var subjectId = _securityContextProvider.GetSubjectId();

        TodoItem todoItem = await _todoItemRepository.GetByIdAsync(request.Id);
        if (todoItem == null)
        {
            throw new TodoItemNotFoundException();
        }
        TodoItemCommonLogic.ValidateTodoItemOwnership(todoItem, subjectId);
        todoItem = await _todoItemRepository.DeleteAsync(todoItem);

        return todoItem.Adapt<TodoItemRes>();
    }
}