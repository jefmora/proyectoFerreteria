using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace AspnetCoreMvcFull.Controllers;

public class AuthController : Controller
{
  public IActionResult ForgotPassword() => View();
  public IActionResult LoginBasic() => View();
  public IActionResult RegisterBasic() => View();
}
