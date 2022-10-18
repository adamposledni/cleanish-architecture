using Microsoft.AspNetCore.Http;
using Onion.App.Logic.Common.Security;
using Onion.Shared.Helpers;
using System.IdentityModel.Tokens.Jwt;

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


    // TODO: RBS
    private SecurityContext BuildSecurityContext()
    {
        var claims = _httpContextAccessor.HttpContext?.User?.Claims;
        if (claims == null || !claims.Any()) return null;

        var sub = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (!string.IsNullOrWhiteSpace(sub) && Guid.TryParse(sub, out var subjectId))
            return new SecurityContext(subjectId);
        return null;
    }

    public Guid GetSubjectId()
    {
        Guard.NotNull(SecurityContext, nameof(SecurityContext));
        return SecurityContext.SubjectId;
    }
}
