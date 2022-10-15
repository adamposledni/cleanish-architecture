using Onion.App.Data.Database.Entities;
using Onion.App.Data.Database.Repositories;
using Onion.App.Logic.Security;
using Onion.App.Logic.TodoLists.Exceptions;
using Onion.App.Logic.TodoLists.Models;
using Onion.Shared.Exceptions;
using Onion.Shared.Helpers;
using Onion.Shared.Mapper;

namespace Onion.App.Logic.TodoLists;

public class TodoListService : ITodoListService
{
    private readonly IObjectMapper _mapper;
    private readonly ISecurityContextProvider _securityContextProvider;
    private readonly ITodoListRepository _todoListRepository;

    public TodoListService(
        IObjectMapper mapper,
        ISecurityContextProvider securityContextProvider,
        ITodoListRepository todoListRepository)
    {
        _mapper = mapper;
        _securityContextProvider = securityContextProvider;
        _todoListRepository = todoListRepository;
    }

    public async Task<TodoListRes> CreateAsync(TodoListReq model)
    {
        var securityContext = _securityContextProvider.SecurityContext;
        Guard.NotNull(securityContext, nameof(securityContext));

        var newTodoList = _mapper.Map<TodoList>(model, tl => tl.UserId = securityContext.SubjectId);
        newTodoList = await _todoListRepository.CreateAsync(newTodoList);

        return _mapper.Map<TodoListRes>(newTodoList);
    }

    public async Task<TodoListRes> GetAsync(Guid todoListId)
    {
        var securityContext = _securityContextProvider.SecurityContext;
        Guard.NotNull(securityContext, nameof(securityContext));

        var todoList = await _todoListRepository.GetByIdAsync(todoListId);
        if (todoList == null) throw new TodoListNotFoundException();
        if (todoList.UserId != securityContext.SubjectId) throw new NotAuthorizedException();

        return _mapper.Map<TodoListRes>(todoList);
    }

    public async Task<IEnumerable<TodoListBriefRes>> ListAsync()
    {
        var securityContext = _securityContextProvider.SecurityContext;
        Guard.NotNull(securityContext, nameof(securityContext));

        var todoLists = await _todoListRepository.ListAsync(securityContext.SubjectId);
        return _mapper.MapCollection<TodoListBriefRes>(todoLists);
    }
}
