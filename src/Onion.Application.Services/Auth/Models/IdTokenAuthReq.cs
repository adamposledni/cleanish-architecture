using System.ComponentModel.DataAnnotations;

namespace Onion.Application.Services.Auth.Models;

public class IdTokenAuthReq
{
    [Required]
    public string IdToken { get; set; }
}
