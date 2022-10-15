using Onion.App.Logic.TodoLists.Models;

namespace Onion.App.Logic.TodoLists;

public interface ITodoListService
{
    Task<TodoListRes> GetAsync(Guid todoListId);
    Task<IEnumerable<TodoListBriefRes>> ListAsync();
    Task<TodoListRes> CreateAsync(TodoListReq model);
}
