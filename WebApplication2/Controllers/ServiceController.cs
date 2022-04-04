using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Data.Entity;
using WebApplication2.Models.Products;

namespace WebApplication2.Controllers;

public class ServiceController : Controller
{
    private ApplicationDbContext _applicationDbContext;

    public ServiceController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> New()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> New(NewProductPost model)
    {
        if (ModelState.IsValid)
        {
            var product = new ProductService()
            {
                Code = model.Code,
                Name = model.Name,
                Price = model.Price,
                ProductType =ProductType.Service,
                UserId = User.GetUserId()
            };
            await _applicationDbContext.ProductServices.AddAsync(product);
            var rows =await _applicationDbContext.SaveChangesAsync();
            return Json(new {status = rows > 0});
        }

        return Json(new {status = false});
    }
    
    
    [HttpGet]
    public async Task<IActionResult> New2()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> New2(NewProductPost model)
    {
        if (ModelState.IsValid)
        {
            var product = new ProductService()
            {
                Code = model.Code,
                Name = model.Name,
                Price = model.Price,
                ProductType =ProductType.Service,
                UserId = User.GetUserId()
            };
            await _applicationDbContext.ProductServices.AddAsync(product);
            var rows =await _applicationDbContext.SaveChangesAsync();
            return Json(new {status = rows > 0});
        }

        return Json(new {status = false});
    }
}