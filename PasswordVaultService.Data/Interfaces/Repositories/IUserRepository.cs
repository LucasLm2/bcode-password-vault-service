using PasswordVaultService.Core.Models;

namespace PasswordVaultService.Data.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(ApplicationUser user);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task UpdateAsync(ApplicationUser user);
    }
}