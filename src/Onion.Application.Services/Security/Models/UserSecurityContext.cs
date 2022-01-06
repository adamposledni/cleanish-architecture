using System;

namespace Onion.Application.Services.Security.Models
{
    public class UserSecurityContext : ISecurityContext
    {
        public bool IsUser => true;
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}
