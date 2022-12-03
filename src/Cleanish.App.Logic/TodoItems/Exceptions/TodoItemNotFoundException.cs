using Cleanish.Shared.Exceptions;

namespace Cleanish.App.Logic.TodoItems.Exceptions;

internal class TodoItemNotFoundException : NotFoundException
{
    public TodoItemNotFoundException() : base("TodoItemNotFound")
    {
    }
}
