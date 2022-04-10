using Onion.Application.DataAccess.Database.Entities.Fields;

namespace Onion.Application.Services.Users.Models;

public class UserRes
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}
