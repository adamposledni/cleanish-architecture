using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Application.Services.Security;
using Onion.Application.Services.TodoItems.Exceptions;
using Onion.Application.Services.TodoLists.Exceptions;
using Onion.Application.Services.TodoLists.Models;
using Onion.Core.Cache;
using Onion.Core.Exceptions;
using Onion.Core.Helpers;
using Onion.Core.Mapper;

namespace Onion.Application.Services.TodoLists;

public class TodoItemService : ITodoItemService
{
    private readonly IDatabaseRepositoryManager _databaseRepositoryManager;
    private readonly IObjectMapper _mapper;
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ITodoListRepository _todoListRepository;

    public TodoItemService(IDatabaseRepositoryManager databaseRepositoryManager, IObjectMapper mapper, ISecurityContextProvider securityContextProvider)
    {
        _databaseRepositoryManager = databaseRepositoryManager;
        _mapper = mapper;
        _securityContextProvider = securityContextProvider;
        _todoItemRepository = _databaseRepositoryManager.GetRepository<ITodoItemRepository, TodoItem>(CacheStrategy.Bypass);
        _todoListRepository = _databaseRepositoryManager.GetRepository<ITodoListRepository, TodoList>(CacheStrategy.Bypass);
    }

    public async Task<TodoItemRes> CreateAsync(TodoItemReq model)
    {
        Guard.NotNull(model, nameof(model));

        // TODO: DRY it
        var securityContext = _securityContextProvider.SecurityContext;
        if (securityContext == null || securityContext.Type != SecurityContextType.User)
            throw new UnauthorizedException();

        bool todoListBelongsToUser = await TodoListBelongsToUserAsync(model.TodoListId, securityContext.SubjectId);
        if (!todoListBelongsToUser) throw new ForbiddenException();

        var newTodoItem = _mapper.Map<TodoItemReq, TodoItem>(model);

        newTodoItem = await _todoItemRepository.CreateAsync(newTodoItem);
        return _mapper.Map<TodoItem, TodoItemRes>(newTodoItem);
    }

    public async Task<TodoItemRes> GetAsync(Guid todoItemId, Guid todoListId)
    {
        var securityContext = _securityContextProvider.SecurityContext;
        if (securityContext == null || securityContext.Type != SecurityContextType.User)
            throw new UnauthorizedException();

        bool todoListBelongsToUser = await TodoListBelongsToUserAsync(todoListId, securityContext.SubjectId);
        if (!todoListBelongsToUser) throw new ForbiddenException();

        var todoItem = await _todoItemRepository.GetByIdAsync(todoItemId);
        if (todoItem == null) throw new TodoItemNotFoundException();

        return _mapper.Map<TodoItem, TodoItemRes>(todoItem);
    }

    public async Task<IEnumerable<TodoItemRes>> ListAsync(Guid todoListId)
    {
        var securityContext = _securityContextProvider.SecurityContext;
        if (securityContext == null || securityContext.Type != SecurityContextType.User)
            throw new UnauthorizedException();

        bool todoListBelongsToUser = await TodoListBelongsToUserAsync(todoListId, securityContext.SubjectId);
        if (!todoListBelongsToUser) throw new ForbiddenException();

        var todoItems = await _todoItemRepository.ListAsync(todoListId);
        return _mapper.MapCollection<TodoItem, TodoItemRes>(todoItems);
    }

    private async Task<bool> TodoListBelongsToUserAsync(Guid todoListId, Guid userId)
    {
        return await _todoListRepository.AnyWithIdAndUserIdAsync(todoListId, userId);
    }
}