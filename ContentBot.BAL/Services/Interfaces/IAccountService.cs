using ContentBot.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.BAL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<APIResponseEntity<RegistrationResponse>> RegisterUser(RegistrationRequestModel registrationRequestModel);

        Task SendEmail(EmailModel emailModel);

        Task<bool> ConfirmEmail(string Token, string Email);

        Task<APIResponseEntity<UserLoginResponseModel>> InitializeLogin(UserLoginRequestModel loginModel);

        APIResponseEntity VerifyLoginOtp(VerifyLoginOtpRequestModel verifyLoginOtpRequestModel);

        Task<APIResponseEntity<ForgotPasswordResponseModel>> ForgotPassword(ForgotPasswordRequestModel forgotPasswordRequestModel);

        Task<APIResponseEntity<ResetPasswordModel>> ResetPassword(ResetPasswordModel resetPasswordModel);

        Task<ApplicationUserModel> GetApplicationUser(string Email);
    }
}
