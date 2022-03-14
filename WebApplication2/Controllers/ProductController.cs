using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication2.Data;
using WebApplication2.Data.Entity;
using WebApplication2.Models.Products;

namespace WebApplication2.Controllers;

[Authorize]
public class ProductController : Controller
{
    private ApplicationDbContext _applicationDbContext;
    public ProductController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public IActionResult NewProduct()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> NewProduct(NewProductPost model)
    {
        if (ModelState.IsValid)
        {
            var product = new ProductService()
            {
                Code = model.Code,
                Name = model.Name,
                Price = model.Price,
                ProductType =ProductType.Product,
                UserId = User.GetUserId()
            };
            await _applicationDbContext.ProductServices.AddAsync(product);
            var rows = _applicationDbContext.SaveChanges();
            if (rows > 0)
            {
                return RedirectToAction("NewProduct");
            }
            ModelState.AddModelError("global","خطا در درج محصول");
        }
        return View(model);
    }

    public async Task<IActionResult> Index()
    {
        var products =await _applicationDbContext
            .ProductServices
            .Where(x=>x.UserId==User.GetUserId())
            .ToListAsync();

        return View(products);
    }  
    
}