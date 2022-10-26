using Onion.Shared.Exceptions;

namespace Onion.App.Logic.TodoItems.Exceptions;

internal class TodoItemNotFoundException : NotFoundException
{
    public TodoItemNotFoundException() : base("TodoItemNotFound")
    {
    }
}
