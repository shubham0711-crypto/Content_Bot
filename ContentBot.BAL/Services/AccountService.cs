using AutoMapper;
using ContentBot.BAL.Services.Interfaces;
using ContentBot.DAL.Entities;
using ContentBot.DAL.Migrations;
using ContentBot.DAL.Repository.Interfaces;
using ContentBot.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace ContentBot.BAL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _memoryCache;
        private readonly ITokenService _tokenService;
        private static Random rand = new Random();


        public AccountService(IAccountRepository accountRepository, IMapper mapper,
            IEmailSender emailSender, IMemoryCache memoryCache, ITokenService tokenService)
        {
            _accountRepository = accountRepository;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<APIResponseEntity<RegistrationResponse>> RegisterUser(RegistrationRequestModel registrationRequestModel)
        {
            try
            {
                APIResponseEntity<RegistrationResponse> AlreadyExists = new APIResponseEntity<RegistrationResponse>();
                var alreadyExist = await _accountRepository.CheckUserAlreadyExists(registrationRequestModel.PhoneNumber, registrationRequestModel.Email);
                if (alreadyExist)
                {
                    AlreadyExists.Message = "Account Already Exists";
                    AlreadyExists.IsSuccess = false;
                    return AlreadyExists;
                }

                ApplicationUser user = new ApplicationUser
                {
                    FirstName = registrationRequestModel.FirstName,
                    LastName = registrationRequestModel.LastName,
                    Email = registrationRequestModel.Email,
                    Password = registrationRequestModel.Password,
                    PhoneNumber = registrationRequestModel.PhoneNumber,
                    ConfirmPassword = registrationRequestModel.ConfirmPassword,
                    EmailConfirmed = false,
                    UserName = registrationRequestModel.UserName,
                    IsActive = false
                };
                var CreatedUser = await _accountRepository.CreateApplicationUser(user, user.Password);

                if (CreatedUser.Succeeded)
                {
                    APIResponseEntity<RegistrationResponse> response = new APIResponseEntity<RegistrationResponse>();
                    string Token = await _accountRepository.GenerateTokenEmailVerificationAsync(user);


                    response.Code = HttpStatusCode.OK;
                    response.Message = "User Succesfully Created";
                    response.Data = new RegistrationResponse
                    {
                        UserID = user.Id,
                        Token = Token,
                        Email = user.Email,
                        Mobile_No = user.PhoneNumber
                    };

                    return response;
                }
                else
                {
                    APIResponseEntity<RegistrationResponse> Error = new APIResponseEntity<RegistrationResponse>();
                    string Errors = "";
                    foreach (var i in CreatedUser.Errors)
                    {
                        Errors += i.Description;
                    }

                    Error.Message = Errors;
                    Error.IsSuccess = false;
                    return Error;
                }
            }
            catch (Exception ex)
            {
                APIResponseEntity<RegistrationResponse> Exception = new APIResponseEntity<RegistrationResponse>();

                Exception.IsSuccess = false;
                Exception.Code = HttpStatusCode.NotFound;
                Exception.Message = ex.Message;
                return Exception;
            }

        }

        public async Task SendEmail(EmailModel emailModel)
        {
            var Body = EmailBody(emailModel.UserName, emailModel.Token);
            await _emailSender.SendEmailAsync(emailModel.Email, "Email Verification", Body);
         
        }

        public async Task<bool> ConfirmEmail(string Token, string Email)
        {
            var user = await _accountRepository.GetUserByEmail(Email);
            if (user == null)
            {
                return false;
            }

            var confirmation = await _accountRepository.ConfirmEmail(user, Token);
            if (confirmation.Succeeded == true)
            {
                return true;
            }

            return false;
        }


        public async Task<APIResponseEntity<UserLoginResponseModel>> InitializeLogin(UserLoginRequestModel loginModel)
        {
            APIResponseEntity<UserLoginResponseModel> response = new APIResponseEntity<UserLoginResponseModel>();
            ApplicationUser applicationUser = new ApplicationUser();

            try
            {
                applicationUser = await _accountRepository.GetUserByEmail(loginModel.Email);
                if (applicationUser != null)
                {
                    var result = await _accountRepository.Login(applicationUser.UserName, loginModel.Password);
                    if (!result.Succeeded)
                    {
                        response.IsSuccess = false;
                        response.Code = HttpStatusCode.OK;
                        response.Message = "Email or password is wrong !!";
                        return response;
                    }
                    else
                    {
                        await SendLoginOTP(applicationUser.Email);
                    }
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, applicationUser.Email),
                        new Claim(ClaimTypes.Role, "Customer"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    AuthenticationModel authenticationModel = new AuthenticationModel
                    {
                        Access_Token = await _tokenService.GenerateAccessToken(claims),
                        Refresh_Access_Token = await _tokenService.GenerateRefreshToken()
                    };

                    response.Code = HttpStatusCode.OK;
                    response.Message = "Login successfully";
                    response.Data = new UserLoginResponseModel
                    {
                        UserId = applicationUser.Id,
                        Name = string.Concat(applicationUser.FirstName, ' ', applicationUser.LastName),
                        Email = applicationUser.Email,
                        AccessToken = authenticationModel.Access_Token,
                    };
                }
                else
                {
                    response.IsSuccess = false;
                    response.Code = HttpStatusCode.OK;
                    response.Message = "Email or password is wrong !!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Code = HttpStatusCode.NotFound;
                response.Message = ex.Message;
                response.Data = new UserLoginResponseModel { Email = applicationUser.Email};
            }
            return response;
        }


        public async Task<APIResponseEntity> SendLoginOTP(string Email)
        {
            APIResponseEntity response = new APIResponseEntity();
            ApplicationUser applicationUser = new ApplicationUser();


            applicationUser = await _accountRepository.GetUserByEmail(Email);

            int EmailOtp = GenerateOTP(6);


            string body = EmailBody(applicationUser.FirstName, EmailOtp);
            await _emailSender.SendEmailAsync(applicationUser.Email, "Send otp", body);

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30));

            _memoryCache.Set(applicationUser.Email, EmailOtp, cacheEntryOptions);

            response.Code = HttpStatusCode.OK;
            response.Message = "Send OTP";
            return response;
        }

        public async Task<APIResponseEntity<ForgotPasswordResponseModel>> ForgotPassword(ForgotPasswordRequestModel forgotPasswordRequestModel)
        {
            APIResponseEntity<ForgotPasswordResponseModel> response = new APIResponseEntity<ForgotPasswordResponseModel>();
            ApplicationUser user = await _accountRepository.GetUserByEmail(forgotPasswordRequestModel.Email);
            if (user != null)
            {
                string code = await _accountRepository.GeneratePasswordResetTokenAsync(user);
                string NewPassword = RandomPassword();
                await _accountRepository.ResetPasswordAsync(user, code, NewPassword);
                await _emailSender.SendEmailAsync(forgotPasswordRequestModel.Email, "New password", $"Your new password is <b> " + NewPassword + "</b>");
                response.Code = HttpStatusCode.OK;
                response.Message = "New Password";
                response.Data = new ForgotPasswordResponseModel { Email = forgotPasswordRequestModel.Email };
            }
            else
            {
                response.Code = HttpStatusCode.OK;
                response.Message = "User Not Found";
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<APIResponseEntity<ResetPasswordModel>> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            APIResponseEntity<ResetPasswordModel> response = new APIResponseEntity<ResetPasswordModel>();
            ApplicationUser applicationUser = await _accountRepository.GetUserByEmail(resetPasswordModel.Email);
            if (applicationUser != null)
            {
                string code = await _accountRepository.GeneratePasswordResetTokenAsync(applicationUser);
                var result = await _accountRepository.ResetPasswordAsync(applicationUser, code, resetPasswordModel.Password);
                if (!result.Succeeded) return null;
                await _emailSender.SendEmailAsync(resetPasswordModel.Email, "Reset password", $"Your password is successfully reset.");

                response.Code = HttpStatusCode.OK;
                response.Message = "Reset password";
                response.Data = new ResetPasswordModel { Email = resetPasswordModel.Email };
            }
            else
            {
                response.Code = HttpStatusCode.OK;
                response.Message = "User Not Found";
                response.IsSuccess = false;
            }
            return response;
        }
    

        public APIResponseEntity VerifyLoginOtp(VerifyLoginOtpRequestModel verifyLoginOtpRequestModel)
        {
            APIResponseEntity response = new APIResponseEntity();

            var EmailOtp = _memoryCache.Get(verifyLoginOtpRequestModel.Email);

            if(verifyLoginOtpRequestModel.EmailOTP == EmailOtp.ToString())
            {
                response.IsSuccess = true;
                response.Code = HttpStatusCode.OK;
                response.Message = "Verify OTP";
            }
            else
            {
                response.IsSuccess = false;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = "OTP mismatched";
            }

            return response;
        }


        private static string EmailBody(string name, string token)
        {
            string filePath = "./StaticFiles/RegSuccess.html";
            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();
            StringBuilder sb = new StringBuilder(mailText);
            sb.Replace("@@FirstName@@", name);
            sb.Replace("@@ButtonLink@@", token);
            string body = sb.ToString();
            return body;
        }

        private static string EmailBody(string name, int Otp)
        {
            string filePath = "./StaticFiles/LogInSuccess.html";
            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();
            StringBuilder sb = new StringBuilder(mailText);
            sb.Replace("@@FirstName@@", name);
            sb.Replace("@@otp@@", Otp.ToString());
            string body = sb.ToString();
            return body;
        }

        public int GenerateOTP(int otpLength)
        {
            string sOTP = string.Empty;
            string sTempChars = string.Empty;
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            Random rand = new Random();

            for (int i = 0; i < otpLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return Convert.ToInt32(sOTP);
        }

       

        public static string RandomPassword(int length = 8)
        {
            string[] categories = {
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                "abcdefghijklmnopqrstuvwxyz",
                "!-_*+&$",
                "0123456789"
            };

            List<char> chars = new List<char>(length);

            // add one char from each category
            foreach (string cat in categories)
            {
                chars.Add(cat[rand.Next(cat.Length)]);
            }

            // add random chars from any category until we hit the length
            string all = string.Concat(categories);
            while (chars.Count < length)
            {
                chars.Add(all[rand.Next(all.Length)]);
            }

            // shuffle and return our password
            var shuffled = chars.OrderBy(c => rand.NextDouble()).ToArray();
            return new string(shuffled);
        }

        public async Task<ApplicationUserModel> GetApplicationUser(string Email)
        {
            ApplicationUser applicationUser = await _accountRepository.GetUserByEmail(Email);
            ApplicationUserModel applicationUserVM = new ApplicationUserModel();
            if (applicationUser != null)
            {
                applicationUserVM = new ApplicationUserModel()
                {
                    Id = applicationUser.Id,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    Email = applicationUser.Email,
                };
            }

            return applicationUserVM;

        }
    }
}
