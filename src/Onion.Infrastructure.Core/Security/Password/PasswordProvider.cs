using Onion.Core.Security;
using System.Security.Cryptography;

namespace Onion.Infrastructure.Core.Security.Password;

public class PasswordProvider : IPasswordProvider
{
    public (byte[] hash, byte[] salt) Hash(string password)
    {
        using HMACSHA256 hmac = new();

        var salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (hash, salt);
    }

    public string Random(int length)
    {
        var alphaLower = "abcdefghijklmnopqrstuvwxyz";
        var alphaUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var numeric = "0123456789";
        var special = ".:?!@#*";

        Random rng = new();
        var chars = (alphaLower + alphaUpper + numeric + special).ToCharArray();
        var password = new char[length];

        for (var i = 0; i < length; i++)
            password[i] = chars[rng.Next(chars.Length - 1)];
        return string.Join(null, chars);
    }

    public bool Verify(string password, byte[] hash, byte[] salt)
    {
        using HMACSHA256 hmac = new(salt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return hash.SequenceEqual(computedHash);
    }
}
