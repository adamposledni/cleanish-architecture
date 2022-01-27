namespace Onion.Core.Security;

public interface IPasswordProvider
{
    (byte[] hash, byte[] salt) Hash(string password);
    string Random(int length);
    bool Verify(string password, byte[] hash, byte[] salt);
}
