

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Data.Entity;
using WebApplication2.Models.Auth;

namespace WebApplication2.Controllers;

public class AuthController : Controller
{
   private ApplicationDbContext _context;

   public AuthController(ApplicationDbContext context)
   {
      _context = context;
   }

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
      if (ModelState.IsValid)
      {
         var checkUser = _context.Users.Any(u => u.Username == model.Username || u.MobileNumber == model.Username);
         if (checkUser)
         {
            ModelState.AddModelError("Username","این شماره قبلا ثبت شده است");
            return View(model);
         }
         var user = new User()
         {
            Username = model.Username,
            MobileNumber = model.Username,
            Password = model.Password
         };
         _context.Users.Add(user);
         var otp = new OtpCode()
         {
            User = user,
            Code = "123456",

         };
         _context.OtpCodes.Add(otp);
         var rows=_context.SaveChanges();
         if (rows > 0)
         {
            return RedirectToAction("CheckOTPCode");
         }
      }
      return View(model);
   }

   public IActionResult CheckOTPCode()
   {
      return View();
   }
   [HttpPost]
   public IActionResult CheckOTPCode(OtpCodePost model)
   {
      if (ModelState.IsValid)
      {
         var otp = _context.OtpCodes
            .Include(x=>x.User)
            .FirstOrDefault(o => o.Code == model.Code);
         
         if (otp == null || otp.IsValid == false)
         {
            ModelState.AddModelError("Code","کد واردشده معتبر نمیباشد");
            return View(model);
         }

        // var user = _context.Users.FirstOrDefault(u => u.Id == otp.User.Id);
        //var u = _context.Users.FirstOrDefault(x => x.Id == 2);
        //var user = _context.Users.Find(2);
        
         otp.User.Status = UserStatus.Active;
         otp.IsUsed = true;
         var row = _context.SaveChanges();
         if (row > 0)
            return RedirectToAction("Login");
      }
      
      return View(model);
   }

   public IActionResult OtpCode()
   {
      var otpCode = _context.OtpCodes.AsNoTracking().ToList();

      return Json(new {name = "ali", familty = "yeganeh", age = 50,otpCode});

      return Content("<strong>OTP</strong>","text/html");
      
      return View("OTP",otpCode);
   }
}