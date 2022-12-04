using FluentValidation;
using MediatR;
using Cleanish.App.Data.Database.Entities;
using Cleanish.App.Data.Database.Repositories;
using Cleanish.App.Logic.Common.Attributes;
using Cleanish.App.Logic.Common.Security;
using Cleanish.App.Logic.TodoLists.Models;

namespace Cleanish.App.Logic.TodoItems.UseCases;

[Authorize]
public class CreateTodoItemRequest : IRequest<TodoItemRes>
{
    public string Title { get; set; }
}

internal class CreateTodoItemRequestValidator : AbstractValidator<CreateTodoItemRequest>
{
    public CreateTodoItemRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
    }
}

internal class CreateTodoItemRequestHandler : IRequestHandler<CreateTodoItemRequest, TodoItemRes>
{
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoItemRepository _todoItemRepository;

    public CreateTodoItemRequestHandler(
        ISecurityContextProvider securityContextProvider,
        ITodoItemRepository todoItemRepository)
    {
        _securityContextProvider = securityContextProvider;
        _todoItemRepository = todoItemRepository;
    }

    public async Task<TodoItemRes> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        Guid subjectId = _securityContextProvider.GetSubjectId();
        TodoItem newTodoItem = request.Adapt<TodoItem>(ti => ti.UserId = subjectId);
        newTodoItem = await _todoItemRepository.CreateAsync(newTodoItem);

        return newTodoItem.Adapt<TodoItemRes>();
    }
}
