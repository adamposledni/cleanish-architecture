using Onion.Application.DataAccess.Database.Entities.Fields;

namespace Onion.Application.Services.Users.Models;

public class Foo1Res
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<Foo2Res> TodoLists { get; set; }
}

public class Foo2Res
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<Foo3Res> TodoItems { get; set; }
}

public class Foo3Res
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TodoItemState State { get; set; }
}