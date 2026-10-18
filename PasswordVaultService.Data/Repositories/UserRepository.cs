using PasswordVaultService.Core.Models;
using PasswordVaultService.Data.Context;
using PasswordVaultService.Data.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PasswordVaultService.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ApplicationUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}