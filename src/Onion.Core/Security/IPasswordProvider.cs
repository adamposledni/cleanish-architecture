namespace Onion.Core.Security
{
    public interface IPasswordProvider
    {
        void Hash(string password, out byte[] hash, out byte[] salt);
        string Random(int length);
        bool Verify(string password, byte[] hash, byte[] salt);
    }
}
