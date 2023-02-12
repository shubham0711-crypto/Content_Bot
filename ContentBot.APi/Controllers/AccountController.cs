using ContentBot.BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentBot.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;   
        }

        //[HttpPost , Route("login")]
        //public async Task<IActionResult> Login()
        //{

        //}
    }
}
