using Microsoft.AspNetCore.Http;
using Onion.Application.DataAccess.Entities;
using Onion.Application.Services.Common;
using Onion.Application.Services.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Onion.WebApi.Services
{
    public class SecurityContextProvider : ISecurityContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Lazy<SecurityContext> _securityContext;

        public SecurityContext SecurityContext => _securityContext.Value;

        public SecurityContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _securityContext = new Lazy<SecurityContext>(() => BuildSecurityContext());
        }

        private SecurityContext BuildSecurityContext()
        {
            var claims = _httpContextAccessor.HttpContext.User?.Claims;
            if (claims == null || !claims.Any()) return null;

            string upn = claims.FirstOrDefault(c => c.Type == ClaimTypes.Upn)?.Value;
            string spn = claims.FirstOrDefault(c => c.Type == ClaimTypes.Spn)?.Value;

            if (!string.IsNullOrWhiteSpace(upn))
            {
                string email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                return new UserSecurityContext { UserId = new Guid(upn), Email = email};
            }
            else if (!string.IsNullOrWhiteSpace(spn))
            {
                return new ClientSecurityContext { ClientId = new Guid(spn) };
            }
            return null;
        }
    }
}
