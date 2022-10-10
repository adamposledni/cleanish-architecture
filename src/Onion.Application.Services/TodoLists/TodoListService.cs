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

        var userId = _securityContextProvider.GetUserId();
        var newTodoList = _mapper.Map<TodoList>(model, tl => tl.UserId = userId);
        newTodoList = await _todoListRepository.CreateAsync(newTodoList);

        return _mapper.Map<TodoListRes>(newTodoList);
    }

    public async Task<TodoListRes> GetAsync(Guid todoListId)
    {
        var userId = _securityContextProvider.GetUserId();
        var todoList = await _todoListRepository.GetByIdAsync(todoListId);
        if (todoList == null) throw new TodoListNotFoundException();
        if (todoList.UserId != userId) throw new ForbiddenException();

        return _mapper.Map<TodoListRes>(todoList);
    }

    public async Task<IEnumerable<TodoListBriefRes>> ListAsync()
    {
        var userId = _securityContextProvider.GetUserId();
        var todoLists = await _todoListRepository.ListAsync(userId);
        return _mapper.MapCollection<TodoListBriefRes>(todoLists);
    }
}
