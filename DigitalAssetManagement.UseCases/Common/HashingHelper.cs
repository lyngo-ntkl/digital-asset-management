namespace DigitalAssetManagement.UseCases.Common
{
    public interface HashingHelper
    {
        void Hash(string value, out string salt, out string hash);
        void Hash(string value, out byte[] salt, out byte[] hash);
        void Hash(string value, byte[] salt, out byte[] hash);
        string Hash(string value, string salt);
    }
}
