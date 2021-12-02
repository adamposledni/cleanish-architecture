using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Common.Models
{
    public abstract class SecurityContext
    {
        public abstract bool IsUser { get; }
    }

    public class UserSecurityContext : SecurityContext
    {
        public override bool IsUser => true;
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }

    public class ClientSecurityContext : SecurityContext
    {
        public override bool IsUser => false;
        public Guid ClientId { get; set; }
    }
}
