using Onion.Application.DataAccess.Database.Entities;
using Onion.Application.DataAccess.Database.Repositories;
using Onion.Application.Services.Security;
using Onion.Application.Services.TodoLists.Exceptions;
using Onion.Application.Services.TodoLists.Models;
using Onion.Core.Cache;
using Onion.Core.Exceptions;
using Onion.Core.Helpers;
using Onion.Core.Mapper;

namespace Onion.Application.Services.TodoLists;

public class TodoListService : ITodoListService
{
    private readonly IDatabaseRepositoryManager _databaseRepositoryManager;
    private readonly IObjectMapper _mapper;
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoListRepository _todoListRepository;

    public TodoListService(IDatabaseRepositoryManager databaseRepositoryManager, IObjectMapper mapper, ISecurityContextProvider securityContextProvider)
    {
        _databaseRepositoryManager = databaseRepositoryManager;
        _mapper = mapper;
        _securityContextProvider = securityContextProvider;
        _todoListRepository = _databaseRepositoryManager.GetRepository<ITodoListRepository, TodoList>(CacheStrategy.Bypass);
    }

    public async Task<TodoListRes> CreateAsync(TodoListReq model)
    {
        Guard.NotNull(model, nameof(model));

        var securityContext = _securityContextProvider.SecurityContext;
        if (securityContext == null || securityContext.Type != SecurityContextType.User)
            throw new UnauthorizedException();

        var newTodoList = _mapper.Map<TodoListReq, TodoList>(model, tl => tl.UserId = securityContext.SubjectId);
        newTodoList = await _todoListRepository.CreateAsync(newTodoList);

        return _mapper.Map<TodoList, TodoListRes>(newTodoList);
    }

    public async Task<TodoListRes> GetAsync(Guid todoListId)
    {
        var securityContext = _securityContextProvider.SecurityContext;
        if (securityContext == null || securityContext.Type != SecurityContextType.User)
            throw new UnauthorizedException();

        bool todoListBelongsToUser = await TodoListBelongsToUserAsync(todoListId, securityContext.SubjectId);
        if (!todoListBelongsToUser) throw new ForbiddenException();

        var todoList = await _todoListRepository.GetByIdAsync(todoListId);
        if (todoList == null) throw new TodoListNotFoundException();

        return _mapper.Map<TodoList, TodoListRes>(todoList);
    }

    public async Task<IEnumerable<TodoListBriefRes>> ListAsync()
    {
        var todoLists = await _todoListRepository.ListAsync();
        return _mapper.MapCollection<TodoList, TodoListBriefRes>(todoLists);
    }

    private async Task<bool> TodoListBelongsToUserAsync(Guid listId, Guid userId)
    {
        return await _todoListRepository.AnyWithIdAndUserIdAsync(listId, userId);
    }
}
