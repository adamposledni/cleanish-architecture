using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Onion.Core.Security;
using Onion.Core.Security.Models;
using System.Threading.Tasks;

namespace Onion.Infrastructure.Security.Google
{
    public class GoogleAuthProvider : IGoogleAuthProvider
    {
        private readonly GoogleAuthSettings _googleAuthSettings;

        public GoogleAuthProvider(IOptions<GoogleAuthSettings> googleAuthSettings)
        {
            _googleAuthSettings = googleAuthSettings.Value;
        }

        public async Task<GoogleIdentity> GetIdentityAsync(string idToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                GoogleJsonWebSignature.ValidationSettings settings = new();
                settings.Audience = new string[] { _googleAuthSettings.ClientId };
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            }
            catch
            {
                return null;
            }

            return new GoogleIdentity()
            {
                Email = payload.Email,
                SubjectId = payload.Subject
            };
        }
    }
}
