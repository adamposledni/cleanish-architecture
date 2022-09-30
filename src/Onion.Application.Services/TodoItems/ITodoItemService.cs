using Onion.Application.Services.TodoLists.Models;

namespace Onion.Application.Services.TodoLists;

public interface ITodoItemService
{
    Task<TodoItemRes> GetAsync(Guid todoItemId, Guid todoListId);
    Task<IEnumerable<TodoItemRes>> ListAsync(Guid todoListId);
    Task<TodoItemRes> CreateAsync(TodoItemReq model);
}
