using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordVaultService.Core.Models;

namespace PasswordVaultService.Data.Configurations
{
    public class PasswordEntryConfiguration : IEntityTypeConfiguration<PasswordEntry>
    {
        public void Configure(EntityTypeBuilder<PasswordEntry> builder)
        {
            builder.HasKey(pe => pe.Id);
            builder.Property(pe => pe.Id).ValueGeneratedOnAdd();

            builder.Property(pe => pe.ServiceName)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(pe => pe.Username)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(pe => pe.EncryptedPassword)
                   .IsRequired()
                   .HasColumnType("bytea");

            builder.Property(pe => pe.EncryptionIV)
                   .IsRequired()
                   .HasMaxLength(16)
                   .HasColumnType("bytea");

            builder.Property(pe => pe.CreatedAt)
                   .IsRequired();

            builder.Property(pe => pe.UpdatedAt)
                   .IsRequired();

            builder.HasOne(pe => pe.User)
                   .WithMany(u => u.PasswordEntries)
                   .HasForeignKey(pe => pe.UserId);
        }
    }
}