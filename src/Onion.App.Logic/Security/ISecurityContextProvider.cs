using Onion.App.Logic.Security.Models;

namespace Onion.App.Logic.Security;

public interface ISecurityContextProvider
{
    SecurityContext SecurityContext { get; }
}
