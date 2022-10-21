using Onion.App.Data.Database.Repositories;
using Onion.Shared.Exceptions;

namespace Onion.App.Logic.TodoItems;

public static class TodoItemCommonLogic
{
    public static async Task ValidateTodoListOwnership(ITodoListRepository todoListRepository, Guid todoListId, Guid subjectId)
    {
        bool todoListBelongsToUser = await todoListRepository.AnyWithIdAndUserIdAsync(todoListId, subjectId);
        if (!todoListBelongsToUser) throw new NotAuthorizedException();
    }
}
