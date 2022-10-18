using FluentValidation;
using MediatR;
using Onion.App.Data.Database.Entities.Fields;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Common.Security;
using Onion.App.Logic.TodoItems.Exceptions;
using Onion.App.Logic.TodoLists.Models;
using Onion.Shared.Mapper;
using System.Threading;

namespace Onion.App.Logic.TodoItems.UseCases;

[Authorize]
public class UpdateTodoItemRequest : IRequest<TodoItemRes>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TodoItemState State { get; set; }
    public Guid TodoListId { get; set; }
}

internal class UpdateTodoItemRequestValidator : AbstractValidator<UpdateTodoItemRequest>
{
    public UpdateTodoItemRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.TodoListId).NotEmpty();
    }
}

internal class UpdateTodoItemRequestHandler : IRequestHandler<UpdateTodoItemRequest, TodoItemRes>
{
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ITodoListRepository _todoListRepository;
    private readonly IObjectMapper _mapper;

    public UpdateTodoItemRequestHandler(
        ISecurityContextProvider securityContextProvider,
        ITodoItemRepository todoItemRepository,
        ITodoListRepository todoListRepository,
        IObjectMapper mapper)
    {
        _securityContextProvider = securityContextProvider;
        _todoItemRepository = todoItemRepository;
        _todoListRepository = todoListRepository;
        _mapper = mapper;
    }

    public async Task<TodoItemRes> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var subjectId = _securityContextProvider.GetSubjectId();

        await TodoItemCommonLogic.ValidateTodoListOwnership(_todoListRepository, request.TodoListId, subjectId);

        var todoItem = await _todoItemRepository.GetByIdAsync(request.Id);
        if (todoItem == null) throw new TodoItemNotFoundException();

        // TODO: update mapper
        todoItem.Title = request.Title;
        todoItem.State = request.State;

        // TODO: optimistic conc
        await Task.Delay(10000);

        todoItem = await _todoItemRepository.UpdateAsync(todoItem);

        return _mapper.Map<TodoItemRes>(todoItem);
    }
}
