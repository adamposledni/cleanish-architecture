using Onion.Shared.Exceptions;

namespace Onion.App.Logic.TodoLists.Exceptions;

internal class TodoListNotFoundException : NotFoundException
{
    public TodoListNotFoundException() : base("TodoListNotFound")
    {
    }
}
