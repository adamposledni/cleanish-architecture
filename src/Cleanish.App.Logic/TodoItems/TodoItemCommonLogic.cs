using Cleanish.App.Data.Database.Entities;
using Cleanish.Shared.Exceptions;

namespace Cleanish.App.Logic.TodoItems;

internal static class TodoItemCommonLogic
{
    public static void ValidateTodoItemOwnership(TodoItem todoItem, Guid userId)
    {
        if (todoItem.UserId != userId) throw new NotAuthorizedException();
    }
}
