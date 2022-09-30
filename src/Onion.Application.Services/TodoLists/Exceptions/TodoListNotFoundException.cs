using Onion.Core.Exceptions;

namespace Onion.Application.Services.TodoLists.Exceptions;

public class TodoListNotFoundException : NotFoundException
{
    private const string MESSAGE_KEY = "TodoListNotFound";
    public TodoListNotFoundException() : base(MESSAGE_KEY)
    {
    }
}
