using Cleanish.App.Data.Database.Entities.Fields;

namespace Cleanish.App.Data.Database.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public UserRole Role { get; set; }

    public IEnumerable<TodoItem> TodoItems { get; set; }
}
