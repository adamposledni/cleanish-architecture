using Onion.Core.Security.Models;

namespace Onion.Core.Security;

public interface IGoogleAuthProvider
{
    Task<GoogleIdentity> GetIdentityAsync(string idToken);
}
