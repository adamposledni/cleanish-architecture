using Microsoft.AspNetCore.Http;
using Onion.Application.Services;
using Onion.Application.Services.Security;
using Onion.Application.Services.Security.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

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

            string sub = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (!string.IsNullOrWhiteSpace(sub) && Guid.TryParse(sub, out Guid subjectId))
            {
                string email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                return new SecurityContext(subjectId, SecurityContextType.User);
            }
            return null;
        }
    }
}
