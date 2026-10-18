using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordVaultService.Core.Models; // Certifique-se de que este using está correto

namespace PasswordVaultService.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(44); 

            builder.Property(u => u.PasswordSalt)
                   .IsRequired()
                   .HasColumnType("bytea");

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            builder.Property(u => u.UpdatedAt)
                   .IsRequired();

            builder.Property(u => u.IsEmailConfirmed)
                   .IsRequired();

            builder.Property(u => u.ConfirmationToken)
                   .HasMaxLength(256);

            builder.Property(u => u.ConfirmationTokenExpiry);

            builder.HasMany(u => u.PasswordEntries)
                   .WithOne(pe => pe.User)
                   .HasForeignKey(pe => pe.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}