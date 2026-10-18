using PasswordVaultService.Business.Interfaces.Services;
using System.Security.Cryptography;

namespace PasswordVaultService.Business.Services
{
    public class EncryptionService : IEncryptionService
    {
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32;  // 256 bits
        private const int Iterations = 10000; // Número de iterações para PBKDF2

        public string HashPassword(string password, out byte[] salt)
        {
            salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(KeySize);
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerifyPassword(string password, string hashedPassword, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(KeySize);
                return Convert.ToBase64String(hash) == hashedPassword;
            }
        }
    }
}