using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Auth.Models
{
    public class AuthRes
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }

        // TODO: roles, refresh token, user name
    }
}
