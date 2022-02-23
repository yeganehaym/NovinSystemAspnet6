

using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models.Auth;

namespace WebApplication2.Controllers;

public class AuthController : Controller
{
   [HttpGet]
   public IActionResult Login()
   {
      return View();
   }

   [HttpPost]
   public IActionResult Login(LoginPost model)
   {
      ModelState.AddModelError("global","نام کاربری یافت نشد");
      return View(model);
   }

   [HttpGet]
   public IActionResult Register()
   {
      return View();
   }
   
   [HttpPost]
   public IActionResult Register(RegisterPost model)
   {
      return View(model);
   }
}