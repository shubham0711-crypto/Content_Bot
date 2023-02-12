using Microsoft.AspNetCore.Mvc;

namespace ContentBot.APi.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
