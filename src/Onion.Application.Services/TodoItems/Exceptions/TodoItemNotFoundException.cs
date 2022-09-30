using Onion.Core.Exceptions;

namespace Onion.Application.Services.TodoItems.Exceptions;

public class TodoItemNotFoundException : NotFoundException
{
    private const string MESSAGE_KEY = "TodoItemNotFound";

    public TodoItemNotFoundException() : base(MESSAGE_KEY)
    {
    }
}
