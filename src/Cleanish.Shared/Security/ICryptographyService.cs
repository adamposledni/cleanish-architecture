namespace Cleanish.Shared.Security;

public interface ICryptographyService
{
    (byte[] hash, byte[] salt) GetHashAndSalt(string value);
    bool VerifyHashAndSalt(string value, byte[] hash, byte[] salt);
    string GetRandomString(int length);
}
