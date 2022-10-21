using Microsoft.AspNetCore.Http;
using Onion.App.Data.Database.Entities.Fields;
using Onion.App.Logic.Common.Security;
using Onion.Shared.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Onion.Pres.WebApi.Security;

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
        var claims = _httpContextAccessor.HttpContext?.User?.Claims;
        if (claims == null || !claims.Any()) return null;

        var subClaim = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var roleClaim = claims.FirstOrDefault(c => c.Type == "role")?.Value;

        if (string.IsNullOrWhiteSpace(subClaim) || 
            !Guid.TryParse(subClaim, out var subjectId) ||
            string.IsNullOrWhiteSpace(roleClaim) ||
            !Enum.TryParse<UserRole>(roleClaim, out var userRole))
        {
            return null;
        }

        return new SecurityContext(subjectId, userRole);
    }

    public Guid GetSubjectId()
    {
        Guard.NotNull(SecurityContext, nameof(SecurityContext));
        return SecurityContext.SubjectId;
    }
}
