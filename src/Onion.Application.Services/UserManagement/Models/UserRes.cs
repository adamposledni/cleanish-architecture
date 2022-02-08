using Onion.Application.DataAccess.Entities.Fields;

namespace Onion.Application.Services.UserManagement.Models;

public class UserRes
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}
