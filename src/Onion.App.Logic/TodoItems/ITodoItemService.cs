using Onion.App.Logic.TodoItems.Models;
using Onion.App.Logic.TodoLists.Models;

namespace Onion.App.Logic.TodoLists;

public interface ITodoItemService
{
    Task<TodoItemRes> GetAsync(Guid todoItemId, Guid todoListId);
    Task<IEnumerable<TodoItemRes>> ListAsync(Guid todoListId);
    Task<TodoItemRes> CreateAsync(TodoItemReq model, Guid todoListId);
    Task<TodoItemRes> UpdateAsync (TodoItemUpdateReq model, Guid todoListId);  
}
