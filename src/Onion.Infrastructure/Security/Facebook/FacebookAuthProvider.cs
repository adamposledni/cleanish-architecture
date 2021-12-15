using Onion.Core.Security;
using Onion.Core.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Infrastructure.Security.Facebook
{
    public class FacebookAuthProvider : IFacebookAuthProvider
    {
        public Task<FacebookIdentity> GetIdentityAsync(string idToken)
        {
            throw new NotImplementedException();
        }
    }
}
