using ContentBot.DAL.Entities;
using ContentBot.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContentBot.DAL.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager; 

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

      
        public async Task<bool> CheckUserAlreadyExists(string phoneNumber, string email)
        {
            var result = await _userManager.Users.Where(x => x.PhoneNumber == phoneNumber || x.Email == email).ToListAsync();

            if (result.Count > 0)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public async Task<IdentityResult> ConfirmEmail(ApplicationUser user, string Token)
        {
           return await _userManager.ConfirmEmailAsync(user, Token);
        }

        public async Task<IdentityResult> CreateApplicationUser(ApplicationUser applicationUser, string password)
        {
            return await _userManager.CreateAsync(applicationUser, password);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);

        }

        public async Task<string> GenerateTokenEmailVerificationAsync(ApplicationUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);

        }

        public async Task<ApplicationUser> GetUserByEmail(string Email)
        {
            return await _userManager.FindByEmailAsync(Email);
        }

        public async Task<SignInResult> Login(string Email, string Password)
        {
            return await _signInManager.PasswordSignInAsync(Email, Password, isPersistent: false, lockoutOnFailure: true);

        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser nuser, string code, string RandomPassword)
        {
            return await _userManager.ResetPasswordAsync(nuser, code, RandomPassword);

        }
    }
}
