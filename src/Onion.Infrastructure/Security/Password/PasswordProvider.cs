using Onion.Core.Security;
using System.Security.Cryptography;

namespace Onion.Infrastructure.Security.Password;

public class PasswordProvider : IPasswordProvider
{
    public (byte[] hash, byte[] salt) Hash(string password)
    {
        using HMACSHA256 hmac = new();

        byte[] salt = hmac.Key;
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (hash, salt);
    }

    public string Random(int length)
    {
        string alphaLower = "abcdefghijklmnopqrstuvwxyz";
        string alphaUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string numeric = "0123456789";
        string special = ".:?!@#*";

        Random rng = new();
        var chars = (alphaLower + alphaUpper + numeric + special).ToCharArray();
        var password = new char[length];

        for (int i = 0; i < length; i++)
        {
            password[i] = chars[rng.Next(chars.Length - 1)];
        }
        return string.Join(null, chars);
    }

    public bool Verify(string password, byte[] hash, byte[] salt)
    {
        using HMACSHA256 hmac = new(salt);

        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return hash.SequenceEqual(computedHash);
    }
}
