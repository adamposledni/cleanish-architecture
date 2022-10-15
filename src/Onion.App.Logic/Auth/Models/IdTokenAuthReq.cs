using System.ComponentModel.DataAnnotations;

namespace Onion.App.Logic.Auth.Models;

public class IdTokenAuthReq
{
    [Required]
    public string IdToken { get; set; }
}
