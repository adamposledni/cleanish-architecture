using System.ComponentModel.DataAnnotations;

namespace Onion.Application.Services.Users.Models;

public class UserReq
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
