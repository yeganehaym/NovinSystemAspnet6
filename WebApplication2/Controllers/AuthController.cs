

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Data.Entity;
using WebApplication2.Models.Auth;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

public class AuthController : Controller
{
   private ApplicationDbContext _context;
   private UserService _userService;
   private IConfiguration _configuration;

   public AuthController(ApplicationDbContext context, UserService userService, IConfiguration configuration)
   {
      _context = context;
      _userService = userService;
      _configuration = configuration;
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
   public async Task<IActionResult> Register(RegisterPost model)
   {
      if (ModelState.IsValid)
      {
         var checkUser =await _userService.IsUserExists(model.Username);
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
         await _userService.AddUserAsync(user);
         var otp = new OtpCode()
         {
            User = user,
            Code = Utils.RandomString(Utils.RandomType.Numbers,6)

         };
        
         await _userService.AddOtpCode(otp);
         var rows=await _context.SaveChangesAsync();
         if (rows > 0)
         {
            var apiKey = _configuration["sms:ghasedak:apikey"];
            var ghasedak = new Ghasedak.Core.Api(apiKey);
            var sendResult = await ghasedak.VerifyAsync(1, "RegtisterCode", new[] {user.MobileNumber}, otp.Code);
            if (sendResult.Result.Code != 200)
            {
               throw new Exception("Send Sms Failed : " + sendResult.Result.Code);
            }
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
   public async Task<IActionResult> CheckOTPCode(OtpCodePost model)
   {
      if (ModelState.IsValid)
      {
         var otp =await _userService.GetOtpCodeAsync(model.Code);
         
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
         var row =await _context.SaveChangesAsync();
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