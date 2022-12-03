namespace Cleanish.App.Data.Security;

public interface ICryptographyService
{
    (byte[] hash, byte[] salt) GetStringHash(string password);
    bool VerifyStringHash(string password, byte[] hash, byte[] salt);
    string GetRandomString(int length);
}
