using Microsoft.EntityFrameworkCore;
using PasswordVaultService.Core.Models;
using PasswordVaultService.Data.Configurations;

namespace PasswordVaultService.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; } = default!;
        public DbSet<PasswordEntry> PasswordEntries { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new PasswordEntryConfiguration());
        }
    }
}