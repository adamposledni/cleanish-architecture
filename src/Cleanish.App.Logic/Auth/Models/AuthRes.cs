using Cleanish.App.Data.Database.Entities.Fields;

namespace Cleanish.App.Logic.Auth.Models;

public class AuthRes
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}


