using Microsoft.AspNetCore.Mvc;

namespace BakeryApp.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}