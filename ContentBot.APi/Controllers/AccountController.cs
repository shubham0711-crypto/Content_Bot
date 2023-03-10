using ContentBot.BAL.Services;
using ContentBot.BAL.Services.Interfaces;
using ContentBot.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ContentBot.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService, IEmailSender emailSender)
        {
            _accountService = accountService;
        }


        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(UserLoginRequestModel loginModel)
        {
            APIResponseEntity<UserLoginResponseModel> response = new APIResponseEntity<UserLoginResponseModel>();

            try
            {
                response = await _accountService.InitializeLogin(loginModel);
                if (response.Code == HttpStatusCode.OK) return Ok(response);
                else return NotFound(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost, Route("verifyLoginOtp")]
        public IActionResult VerifyLoginOtp(VerifyLoginOtpRequestModel verifyLoginOtpRequestModel)
        {
            APIResponseEntity response = new APIResponseEntity();
            try
            {
                response = _accountService.VerifyLoginOtp(verifyLoginOtpRequestModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return NotFound();
            }
        }

        [HttpPost, Route("Register")]
        public async Task<IActionResult> Register(RegistrationRequestModel requestModel)
        {
            APIResponseEntity<RegistrationResponse> result = new APIResponseEntity<RegistrationResponse>();

            try
            {
                result = await _accountService.RegisterUser(requestModel);

                if (result.IsSuccess)
                {
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { result.Data.Token, Email = result.Data.Email }, Request.Scheme);

                    EmailModel email = new EmailModel
                    {
                        UserName = requestModel.UserName,
                        Token = confirmationLink,
                        Email = requestModel.Email
                    };
                    await _accountService.SendEmail(email);
                }

                return Ok(result);
            }

            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }



        [HttpGet, Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string Token, string Email)
        {
            var result = await _accountService.ConfirmEmail(Token, Email);
            return Ok(result);
        }

        [HttpPost, Route("forgotpassword")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequestModel  forgotPasswordModel)
        {
            APIResponseEntity<ForgotPasswordResponseModel> response = new APIResponseEntity<ForgotPasswordResponseModel>();
            try
            {
                response = await _accountService.ForgotPassword(forgotPasswordModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost, Route("resetpassword")]
        public async Task<IActionResult> ResetPasswordAsync(Models.Models.ResetPasswordModel resetPasswordModel)
        {
            APIResponseEntity<Models.Models.ResetPasswordModel> response = new APIResponseEntity<Models.Models.ResetPasswordModel>();
            try
            {
                response = await _accountService.ResetPassword(resetPasswordModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet, Route("getProfileDetail")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProfileDetail(string Email)
        {

            APIResponseEntity<ApplicationUserModel> response = new APIResponseEntity<ApplicationUserModel>();
            try
            {
                ApplicationUserModel applicationUserVM = await _accountService.GetApplicationUser(Email);
                return Ok(applicationUserVM);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
