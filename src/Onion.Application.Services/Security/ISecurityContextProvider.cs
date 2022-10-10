using Onion.Application.Services.Security.Models;

namespace Onion.Application.Services.Security;

public interface ISecurityContextProvider
{
    SecurityContext SecurityContext { get; }
    Guid GetUserId();
}
