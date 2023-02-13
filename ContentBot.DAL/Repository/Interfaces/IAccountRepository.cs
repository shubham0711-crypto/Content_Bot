using ContentBot.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.DAL.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> CheckUserAlreadyExists(string phoneNumber, string email);

        Task<IdentityResult> CreateApplicationUser(ApplicationUser applicationUser, string password);

        Task<string> GenerateTokenEmailVerificationAsync(ApplicationUser user);

        Task<ApplicationUser> GetUserByEmail(string Email);

        Task<IdentityResult> ConfirmEmail(ApplicationUser user, string Token);

        Task<SignInResult> Login(string Email, string Password);        

    }
}
