using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Onion.App.Data.Security;
using Onion.App.Data.Security.Models;
using Onion.Shared.Helpers;

namespace Onion.Impl.App.Data.Security.Google;

public class GoogleAuthProvider : IGoogleAuthProvider
{
    private readonly GoogleAuthSettings _googleAuthSettings;

    public GoogleAuthProvider(IOptions<GoogleAuthSettings> googleAuthSettings)
    {
        _googleAuthSettings = googleAuthSettings.Value;
    }

    public async Task<GoogleIdentity> GetIdentityAsync(string idToken)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(idToken, nameof(idToken));

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
