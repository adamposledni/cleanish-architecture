using Onion.Core.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Core.Security
{
    public interface IGoogleAuthProvider
    {
        Task<GoogleIdentity> GetIdentityAsync(string idToken);
    }
}
