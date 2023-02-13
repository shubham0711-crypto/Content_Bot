using AutoMapper;
using ContentBot.BAL.Services.Interfaces;
using ContentBot.DAL.Entities;
using ContentBot.DAL.Repository.Interfaces;
using ContentBot.Models.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ContentBot.BAL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;


        public AccountService(IAccountRepository accountRepository, IMapper mapper, IEmailSender emailSender)
        {
            _accountRepository = accountRepository;
            _emailSender = emailSender;
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
            //await _emailSender.SendEmailAsync(emailModel.Email,"Email Verification" , Body);

            using(MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("shubhamchoksi15@gmail.com");
                mail.To.Add(emailModel.Email);
                mail.Subject = "EmailConfirmation";
                mail.Body = Body;
                mail.IsBodyHtml= true;
                using(SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("shubhamchoksi15@gmail.com", "zkjqgdegajwpcikw");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public async Task<bool> ConfirmEmail(string Token, string Email)
        {
            var user = await _accountRepository.GetUserByEmail(Email);
            if(user == null) {
                return false;         
            }

            var confirmation = await _accountRepository.ConfirmEmail(user, Token);  
            if(confirmation.Succeeded == true)
            {
                return true;
            }

            return false;
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

      
    }
}
