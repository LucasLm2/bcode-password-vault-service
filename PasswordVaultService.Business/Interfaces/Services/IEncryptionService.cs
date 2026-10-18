namespace PasswordVaultService.Business.Interfaces.Services
{
    public interface IEncryptionService
    {
        string HashPassword(string password, out byte[] salt);
        bool VerifyPassword(string password, string hashedPassword, byte[] salt);
    }
}