using ContentBot.BAL.Services.Interfaces;
using ContentBot.Models.Models;
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



        [HttpPost, Route("Register")]
        public async Task<IActionResult> Register(RegistrationRequestModel requestModel)
        {
            APIResponseEntity<RegistrationResponse> result = new APIResponseEntity<RegistrationResponse>();

            try
            {
                result = await _accountService.RegisterUser(requestModel);

                return Ok(result);
            }

            catch(Exception ex) 
            {
                result.IsSuccess = false;
                result.Code = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        [HttpPost, Route("SendEmail")]
        public async Task<IActionResult> SendEmail(EmailModel confirmEmailModel)
        {
            var result = await _accountService.SendEmail(confirmEmailModel);
        }
    }
}
