using Onion.Application.Services.TodoLists.Models;

namespace Onion.Application.Services.TodoLists;

public interface ITodoListService
{
    Task<TodoListRes> GetAsync(Guid todoListId);
    Task<IEnumerable<TodoListBriefRes>> ListAsync();
    Task<TodoListRes> CreateAsync(TodoListReq model);
}
