using Onion.App.Data.Security.Models;

namespace Onion.App.Data.Security;

public interface IGoogleAuthProvider
{
    Task<GoogleIdentity> GetIdentityAsync(string idToken);
}
