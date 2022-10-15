using Onion.App.Data.Security;
using Onion.Shared.Helpers;
using System.Security.Cryptography;

namespace Onion.Impl.App.Data.Security.Crypto;

public class CryptographyService : ICryptographyService
{
    public (byte[] hash, byte[] salt) GetStringHash(string password)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(password, nameof(password));

        using HMACSHA256 hmac = new();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (hash, salt);
    }

    public bool VerifyStringHash(string password, byte[] hash, byte[] salt)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(password, nameof(password));
        Guard.NotNullOrEmpty(hash, nameof(hash));
        Guard.NotNullOrEmpty(salt, nameof(salt));

        using HMACSHA256 hmac = new(salt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return hash.SequenceEqual(computedHash);
    }

    public string GetRandomString(int bytes)
    {
        Guard.Min(bytes, 0, nameof(bytes));

        var randomNumber = new byte[bytes];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
