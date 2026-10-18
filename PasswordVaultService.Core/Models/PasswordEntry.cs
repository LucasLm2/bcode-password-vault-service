namespace PasswordVaultService.Core.Models
{
    public class PasswordEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ServiceName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public byte[] EncryptedPassword { get; set; } = Array.Empty<byte>();
        public byte[] EncryptionIV { get; set; } = Array.Empty<byte>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}