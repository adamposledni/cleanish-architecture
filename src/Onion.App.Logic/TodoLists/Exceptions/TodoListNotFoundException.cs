using Onion.Shared.Exceptions;

namespace Onion.App.Logic.TodoLists.Exceptions;

public class TodoListNotFoundException : NotFoundException
{
    public TodoListNotFoundException() : base("TodoListNotFound")
    {
    }
}
