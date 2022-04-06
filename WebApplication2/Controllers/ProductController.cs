using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication2.Data;
using WebApplication2.Data.Entity;
using WebApplication2.Models.Products;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[Authorize]
public class ProductController : Controller
{
    private ApplicationDbContext _applicationDbContext;
    private ProductServices _productServices;
    public ProductController(ApplicationDbContext applicationDbContext, ProductServices productServices)
    {
        _applicationDbContext = applicationDbContext;
        _productServices = productServices;
    }
    
    [Authorize()]
    public IActionResult NewProduct()
    {
        
        return View();
    }
    
    [Authorize()]
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

    public async Task<IActionResult> Products()
    {
        return View();
    }

    public async Task<IActionResult> LoadProducts(int start,int length,int draw)
    {
        var search = Request.Query["search[value]"].ToString();
        
        var products = await _productServices.GetProducts(start, length,search);
        var totalCount = await _productServices.GetProductsCount(null);
        var filtercount = totalCount;
        if(!string.IsNullOrEmpty(search))
            filtercount=await _productServices.GetProductsCount(search);

        var items = products
            .Select(x => new ProductGet()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Price = x.Price
            });
        return Json(new
        {
            data = items,
            draw,
            recordsTotal = totalCount,
            recordsFiltered = filtercount
        });
    }

    [HttpPost]
    public async Task<IActionResult> RemoveProduct(int id)
    {
        await _productServices.RemoveProduct(id);
        var rows = await _applicationDbContext.SaveChangesAsync();
        return Json(new {status = rows > 0});
    }
}