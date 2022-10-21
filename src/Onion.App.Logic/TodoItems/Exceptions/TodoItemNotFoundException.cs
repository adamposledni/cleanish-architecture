using Onion.Shared.Exceptions;

namespace Onion.App.Logic.TodoItems.Exceptions;

public class TodoItemNotFoundException : NotFoundException
{
    public TodoItemNotFoundException() : base("TodoItemNotFound")
    {
    }
}
