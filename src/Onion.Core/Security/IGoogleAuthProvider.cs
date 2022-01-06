using Onion.Core.Security.Models;
using System.Threading.Tasks;

namespace Onion.Core.Security
{
    public interface IGoogleAuthProvider
    {
        Task<GoogleIdentity> GetIdentityAsync(string idToken);
    }
}
