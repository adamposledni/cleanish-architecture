using FluentValidation;
using MediatR;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Entities.Fields;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Logic.Common.Attributes;
using Cleanish.App.Logic.Common.Security;
using Cleanish.App.Logic.TodoItems.Exceptions;
using Cleanish.App.Logic.TodoLists.Models;

namespace Cleanish.App.Logic.TodoItems.UseCases;

[Authorize]
public class UpdateTodoItemRequest : IRequest<TodoItemRes>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TodoItemState State { get; set; }
}

internal class UpdateTodoItemRequestValidator : AbstractValidator<UpdateTodoItemRequest>
{
    public UpdateTodoItemRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
    }
}

internal class UpdateTodoItemRequestHandler : IRequestHandler<UpdateTodoItemRequest, TodoItemRes>
{
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoItemRepository _todoItemRepository;

    public UpdateTodoItemRequestHandler(
        ISecurityContextProvider securityContextProvider,
        ITodoItemRepository todoItemRepository)
    {
        _securityContextProvider = securityContextProvider;
        _todoItemRepository = todoItemRepository;
    }

    public async Task<TodoItemRes> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        Guid subjectId = _securityContextProvider.GetSubjectId();

        TodoItem todoItem = await _todoItemRepository.GetByIdAsync(request.Id);
        if (todoItem == null) throw new TodoItemNotFoundException();
        TodoItemCommonLogic.ValidateTodoItemOwnership(todoItem, subjectId);
        UpdateFields(todoItem, request);
        todoItem = await _todoItemRepository.UpdateAsync(todoItem);

        return todoItem.Adapt<TodoItemRes>();
    }

    private void UpdateFields(TodoItem entity, UpdateTodoItemRequest request)
    {
        entity.Title = request.Title;
        entity.State = request.State;
    }
}
