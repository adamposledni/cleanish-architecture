using Onion.App.Data.Database.Entities.Fields;

namespace Onion.App.Logic.Users.Models;

public class Foo1Res
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public int Foo { get; set; }
    public string FooString { get; set; }
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