using System.ComponentModel.DataAnnotations;

namespace Onion.App.Logic.Auth.Models;

public class RefreshTokenReq
{
    [Required]
    public string RefreshToken { get; set; }
}
