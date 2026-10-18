namespace PasswordVaultService.Core.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsEmailConfirmed { get; set; } = false;
        public string? ConfirmationToken { get; set; }
        public DateTime? ConfirmationTokenExpiry { get; set; }

        public ICollection<PasswordEntry> PasswordEntries { get; set; } = new List<PasswordEntry>();
    }
}