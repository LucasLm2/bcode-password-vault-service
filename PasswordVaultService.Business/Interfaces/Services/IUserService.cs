using PasswordVaultService.Core.Models;

namespace PasswordVaultService.Business.Interfaces.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> RegisterUserAsync(string name, string email, string password);
        Task<bool> IsEmailTakenAsync(string email);
        Task<ApplicationUser?> AuthenticateUserAsync(string email, string password);
    }
}