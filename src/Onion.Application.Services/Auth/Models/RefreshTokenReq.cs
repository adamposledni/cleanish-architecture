using System.ComponentModel.DataAnnotations;

namespace Onion.Application.Services.Auth.Models;

public class RefreshTokenReq
{
    [Required]
    public string RefreshToken { get; set; }
}
