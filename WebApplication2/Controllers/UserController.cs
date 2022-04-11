using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication2.Data;
using WebApplication2.Models.Users;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[Authorize]
public class UserController:Controller
{
    private IWebHostEnvironment _environment;
    private UserService _userService;
    private ApplicationDbContext _context;

    public UserController(IWebHostEnvironment environment, UserService userService, ApplicationDbContext context)
    {
        _environment = environment;
        _userService = userService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Settings()
    {
        var userId = User.GetUserId();
        var user = await _userService.FindUserAsync(userId);
        var vm = new SettingsViewModel()
        {
            FirstName = user.FirstName,
            LastName = user.LastName
        };
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Settings(SettingsViewModel model)
    {
        var userId = User.GetUserId();
        var user = await _userService.FindUserAsync(userId);
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        if (model.Avatar.Length > 0)
        {
            var root = _environment.ContentRootPath;
            var path = Path.Combine(root, "pictures");
            Directory.CreateDirectory(path);
            var ext = Path.GetExtension(model.Avatar.FileName);
            var fileName = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + ext;

            var fullName = Path.Combine(path, fileName);
            var fileStream = new FileStream(fullName, FileMode.Create);
            await model.Avatar.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            fileStream.Close();
            
            user.ImageProfile = fileName;
        }
        await _context.SaveChangesAsync();
        return RedirectToAction("Settings");
    }

    public async Task<IActionResult> Avatar()
    {
        var userId = User.GetUserId();
        var user = await _userService.FindUserAsync(userId);
        var fileName = user.ImageProfile;
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = "default.png";
        }
        var path = Path.Combine(_environment.ContentRootPath, "pictures", fileName);
        var fileStream = new FileStream(path, FileMode.Open);
        return File(fileStream, "image/jpg");
    }
}
