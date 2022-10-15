using System.ComponentModel.DataAnnotations;

namespace Onion.App.Logic.Auth.Models;

public class PasswordAuthReq
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
