using Onion.Core.Helpers;
using Onion.Core.Security;
using System.Security.Cryptography;

namespace Onion.Infrastructure.Core.Security.Password;

public class PasswordProvider : IPasswordProvider
{
    public (byte[] hash, byte[] salt) Hash(string password)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(password, nameof(password));

        using HMACSHA256 hmac = new();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (hash, salt);
    }

    public bool Verify(string password, byte[] hash, byte[] salt)
    {
        Guard.NotNullOrEmptyOrWhiteSpace(password, nameof(password));
        Guard.NotNullOrEmpty(hash, nameof(hash));
        Guard.NotNullOrEmpty(salt, nameof(salt));

        using HMACSHA256 hmac = new(salt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return hash.SequenceEqual(computedHash);
    }

    public string Random(int length)
    {
        Guard.Min(length, 1, nameof(length));

        var alphaLower = "abcdefghijklmnopqrstuvwxyz";
        var alphaUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var numeric = "0123456789";
        var special = ".:?!@#*";

        Random rng = new();
        var chars = (alphaLower + alphaUpper + numeric + special).ToCharArray();

        StringBuilder passwordBuilder = new();
        for (var i = 0; i < length; i++)
            passwordBuilder.Append(chars[rng.Next(chars.Length - 1)]);
        return passwordBuilder.ToString();
    }
}
