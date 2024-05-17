using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
