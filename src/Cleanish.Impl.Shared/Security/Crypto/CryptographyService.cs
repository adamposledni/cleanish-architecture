using Cleanish.Shared.Helpers;
using Cleanish.Shared.Security;
using System.Security.Cryptography;

namespace Cleanish.Impl.Shared.Security.Crypto;

internal class CryptographyService : ICryptographyService
{
    public (byte[] hash, byte[] salt) GetHashAndSalt(string value)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(value, nameof(value));

        using HMACSHA256 hmac = new();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
        return (hash, salt);
    }

    public bool VerifyHashAndSalt(string value, byte[] hash, byte[] salt)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(value, nameof(value));
        Guard.NotNullOrEmpty(hash, nameof(hash));
        Guard.NotNullOrEmpty(salt, nameof(salt));

        using HMACSHA256 hmac = new(salt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
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
