using PasswordVaultService.Business.Interfaces.Services;
using PasswordVaultService.Core.Models;
using PasswordVaultService.Data.Interfaces.Repositories;

namespace PasswordVaultService.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;

        public UserService(IUserRepository userRepository, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        public async Task<ApplicationUser> RegisterUserAsync(string name, string email, string password)
        {
            if (await IsEmailTakenAsync(email))
            {
                // Poderíamos lançar uma exceção de negócio aqui,
                // como PasswordVaultService.Core.Exceptions.BusinessException
                throw new InvalidOperationException("Este e-mail já está em uso.");
            }

            // Gerar o salt e o hash da senha
            byte[] salt;
            string hashedPassword = _encryptionService.HashPassword(password, out salt);

            var newUser = new ApplicationUser
            {
                Name = name,
                Email = email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt
            };

            await _userRepository.AddAsync(newUser);
            return newUser;
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email) != null;
        }

        public async Task<ApplicationUser?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !_encryptionService.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                return null; 
            }

            // Opcional: Se IsEmailConfirmed for 'false' e você quiser bloquear o login até a confirmação
            // if (!user.IsEmailConfirmed)
            // {
            //     // Lança uma exceção específica ou retorna null para indicar que o e-mail não foi confirmado
            //     throw new InvalidOperationException("Seu e-mail ainda não foi confirmado. Por favor, verifique sua caixa de entrada.");
            // }

            // Atualiza o UpdatedAt sempre que o usuário faz login com sucesso (opcional, mas boa prática)
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return user;
        }
    }
}