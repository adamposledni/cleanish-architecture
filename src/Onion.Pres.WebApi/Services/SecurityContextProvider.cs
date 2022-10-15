using Microsoft.AspNetCore.Http;
using Onion.App.Logic;
using Onion.App.Logic.Security;
using Onion.App.Logic.Security.Models;
using Onion.Shared.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace Onion.Pres.WebApi.Services;

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

        string sub = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (!string.IsNullOrWhiteSpace(sub) && Guid.TryParse(sub, out Guid subjectId))
        {
            return new SecurityContext(subjectId);
        }
        return null;
    }
}
