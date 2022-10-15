using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Security;
using Onion.App.Logic.TodoItems.Exceptions;
using Onion.App.Logic.TodoItems.Models;
using Onion.App.Logic.TodoLists.Models;
using Onion.Shared.Exceptions;
using Onion.Shared.Helpers;
using Onion.Shared.Mapper;

namespace Onion.App.Logic.TodoLists;

public class TodoItemService : ITodoItemService
{
    private readonly IObjectMapper _mapper;
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ITodoListRepository _todoListRepository;

    public TodoItemService(
        IObjectMapper mapper,
        ISecurityContextProvider securityContextProvider,
        ITodoItemRepository todoItemRepository,
        ITodoListRepository todoListRepository)
    {
        _mapper = mapper;
        _securityContextProvider = securityContextProvider;
        _todoItemRepository = todoItemRepository;
        _todoListRepository = todoListRepository;
    }

    public async Task<TodoItemRes> CreateAsync(TodoItemReq model, Guid todoListId)
    {
        var securityContext = _securityContextProvider.SecurityContext;
        Guard.NotNull(securityContext, nameof(securityContext));

        await ValidateTodoListOwnership(todoListId, securityContext.SubjectId);

        var newTodoItem = _mapper.Map<TodoItem>(model);
        newTodoItem = await _todoItemRepository.CreateAsync(newTodoItem);
        return _mapper.Map<TodoItemRes>(newTodoItem);
    }

    public async Task<TodoItemRes> GetAsync(Guid todoItemId, Guid todoListId)
    {
        var securityContext = _securityContextProvider.SecurityContext;
        Guard.NotNull(securityContext, nameof(securityContext));

        await ValidateTodoListOwnership(todoListId, securityContext.SubjectId);

        var todoItem = await _todoItemRepository.GetByIdAsync(todoItemId);
        if (todoItem == null) throw new TodoItemNotFoundException();

        return _mapper.Map<TodoItemRes>(todoItem);
    }

    public async Task<IEnumerable<TodoItemRes>> ListAsync(Guid todoListId)
    {
        var securityContext = _securityContextProvider.SecurityContext;
        Guard.NotNull(securityContext, nameof(securityContext));

        await ValidateTodoListOwnership(todoListId, securityContext.SubjectId);

        var todoItems = await _todoItemRepository.ListAsync(todoListId);
        return _mapper.MapCollection<TodoItemRes>(todoItems);
    }

    // TODO: optimistic conc
    // TODO: update mapper
    public async Task<TodoItemRes> UpdateAsync(TodoItemUpdateReq model, Guid todoListId)
    {
        var securityContext = _securityContextProvider.SecurityContext;
        Guard.NotNull(securityContext, nameof(securityContext));

        await ValidateTodoListOwnership(todoListId, securityContext.SubjectId);

        var todoItem = await _todoItemRepository.GetByIdAsync(model.Id);
        if (todoItem == null) throw new TodoItemNotFoundException();

        todoItem.Title = model.Title;
        todoItem.State = model.State;

        await Task.Delay(10000);

        todoItem = await _todoItemRepository.UpdateAsync(todoItem);

        return _mapper.Map<TodoItemRes>(todoItem);
    }

    private async Task ValidateTodoListOwnership(Guid todoListId, Guid userId)
    {
        bool todoListBelongsToUser = await _todoListRepository.AnyWithIdAndUserIdAsync(todoListId, userId);
        if (!todoListBelongsToUser) throw new NotAuthorizedException();
    }
}