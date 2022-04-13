

using System.Reflection;
using System.Security.Claims;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Data.Domains;
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
   public IActionResult Login(string? returnUrl)
   {
      var loginpost = new LoginPost()
      {
         ReturnUrl = returnUrl
      };
      return View(loginpost);
   }

   [HttpPost]
   public async Task<IActionResult> Login(LoginPost model)
   {
      if (ModelState.IsValid)
      {
         var user = await _userService.LoginAsync(model.Username, model.Password);
         if (user == null)
         {
            ModelState.AddModelError("global","کاربر مورد نظر یافت نشد");
            return View(model);
         }

         ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
         claimsIdentity.AddClaim(new Claim(ClaimTypes.Name,user.Username));
         claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()));
         claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName,user.FirstName + " " + user.LastName));
         claimsIdentity.AddClaim(new Claim(ClaimTypes.SerialNumber,user.SerialNo));

         var roles =await _userService.GetRolesAsync(user.Id);

         foreach (var role in roles)
         {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role,role.Name));
         }
         if(user.IsAdmin)
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role,"admin"));
         
         var prinicipal = new ClaimsPrincipal(claimsIdentity);
         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, prinicipal);
         
         if(String.IsNullOrEmpty(model.ReturnUrl))
            return RedirectToAction("Index", "Home");
         return Redirect(model.ReturnUrl);
      }
      return View(model);
   }

   public async Task<IActionResult> Logout()
   {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Login");
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
         var user =await _userService.GetUserAsync(model.Username);
         if (user!=null && user.Status!=UserStatus.None)
         {
            ModelState.AddModelError("Username","این شماره قبلا ثبت شده است");
            return View(model);
         }
         else if (user==null)
         {
            user = new User()
            {
               Username = model.Username,
               MobileNumber = model.Username,
               Password = model.Password,
               SerialNo = Utils.RandomString(Utils.RandomType.All,10)
            };
            await _userService.AddUserAsync(user);
         }
         
         var otp = new OtpCode()
         {
            User = user,
            Code = Utils.RandomString(Utils.RandomType.Numbers,6)

         };
        
         await _userService.AddOtpCode(otp);
         var rows=await _context.SaveChangesAsync();
         if (rows > 0)
         {
            BackgroundJob.Enqueue(() => SendSms(user.MobileNumber,otp.Code));
            return RedirectToAction("CheckOTPCode");
         }
      }
      return View(model);
   }

   public async Task SendSms(string mobile,string otpCode)
   {
      var apiKey = _configuration["sms:ghasedak:apikey"];
      var ghasedak = new Ghasedak.Core.Api(apiKey);
      await ghasedak.VerifyAsync(1, "RegtisterCode", new[] {mobile}, otpCode);
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

   public async Task<IActionResult> AccessList()
   {
      var result = Assembly.GetExecutingAssembly()
         .GetTypes()
         .Where(type => typeof(Controller).IsAssignableFrom(type))
         .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
         .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
         .GroupBy(x => x.DeclaringType)
         .Select(x => new AccessList(){ Controller = x.Key.Name, Actions = x.Select(s => s.Name).ToList() })
         .ToList();
      
      return View(result);
   }
}