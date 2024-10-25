using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public interface HashingHelper
    {
        void Hash(string value, out string salt, out string hash);
        void Hash(string value, out byte[] salt, out byte[] hash);
        void Hash(string value, byte[] salt, out byte[] hash);
        string Hash(string value, string salt);
    }

    public class HashingHelperImplementation: HashingHelper
    {
        private readonly IConfiguration _configuration;
        private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

        public HashingHelperImplementation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Hash(string value, out string salt, out string hash)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(int.Parse(_configuration.GetSection("hashing:saltByteSize").Value!));
            var hashing = new Rfc2898DeriveBytes(value, saltBytes, int.Parse(_configuration.GetSection("hashing:iteration").Value!), _hashAlgorithm);
            salt = Convert.ToBase64String(saltBytes);
            hash = Convert.ToBase64String(hashing.GetBytes(int.Parse(_configuration.GetSection("hashing:hashByteSize").Value!)));
        }

        public void Hash(string value, out byte[] salt, out byte[] hash)
        {
            salt = RandomNumberGenerator.GetBytes(int.Parse(_configuration.GetSection("hashing:saltByteSize").Value!));
            var hashing = new Rfc2898DeriveBytes(value, salt, int.Parse(_configuration.GetSection("hashing:iteration").Value!), _hashAlgorithm);
            hash = hashing.GetBytes(int.Parse(_configuration.GetSection("hashing:hashByteSize").Value!));
        }

        public void Hash(string value, byte[] salt, out byte[] hash)
        {
            var hashing = new Rfc2898DeriveBytes(value, salt, int.Parse(_configuration.GetSection("hashing:iteration").Value!), _hashAlgorithm);
            hash = hashing.GetBytes(int.Parse(_configuration.GetSection("hashing:hashByteSize").Value!));
        }

        public string Hash(string value, string salt)
        {
            var hashing = new Rfc2898DeriveBytes(value, Convert.FromBase64String(salt), int.Parse(_configuration.GetSection("hashing:iteration").Value!), _hashAlgorithm);
            var hash = Convert.ToBase64String(hashing.GetBytes(int.Parse(_configuration.GetSection("hashing:hashByteSize").Value!)));
            return hash;
        }
    }
}
