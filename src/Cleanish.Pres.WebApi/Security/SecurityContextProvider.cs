using Microsoft.AspNetCore.Http;
using Cleanish.App.Data.Database.Entities.Fields;
using Cleanish.App.Logic.Common.Security;
using Cleanish.Shared.Helpers;

namespace Cleanish.Pres.WebApi.Security;

internal class SecurityContextProvider : ISecurityContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Lazy<SecurityContext> _securityContext;
    public SecurityContext SecurityContext => _securityContext.Value;

    public SecurityContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _securityContext = new Lazy<SecurityContext>(BuildSecurityContext);
    }

    private SecurityContext BuildSecurityContext()
    {
        var claims = _httpContextAccessor.HttpContext?.User?.Claims;
        if (claims == null || !claims.Any()) return null;

        var subClaim = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var roleClaim = claims.FirstOrDefault(c => c.Type == "role")?.Value;

        if (string.IsNullOrWhiteSpace(subClaim) || 
            !Guid.TryParse(subClaim, out Guid subjectId) ||
            string.IsNullOrWhiteSpace(roleClaim) ||
            !Enum.TryParse(roleClaim, out UserRole userRole))
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
