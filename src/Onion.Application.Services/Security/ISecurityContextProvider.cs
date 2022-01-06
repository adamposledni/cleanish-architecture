using Onion.Application.Services.Security.Models;

namespace Onion.Application.Services.Security
{
    public interface ISecurityContextProvider
    {
        ISecurityContext SecurityContext { get; }
    }
}
