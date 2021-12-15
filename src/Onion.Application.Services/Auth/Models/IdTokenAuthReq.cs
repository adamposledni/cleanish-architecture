using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Auth.Models
{
    public class IdTokenAuthReq
    {
        [Required]
        public string IdToken { get; set; }
    }
}
