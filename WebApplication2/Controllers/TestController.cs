using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Data.Domains;
using WebApplication2.Data.Entity;

namespace WebApplication2.Controllers;

public class TestController : Controller
{
    private ApplicationDbContext _applicationDbContext;

    public TestController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public IActionResult AddData()
    {
        _applicationDbContext.ProductServices.Add(new ProductService()
        {
            Name = "شست و شو بدنه خودرو",
            Price = 10000,
            Code = "1245",
            ProductType = ProductType.Service
        });
        
        _applicationDbContext.ProductServices.Add(new ProductService()
        {
            Name = "نظافت داخلی خودرو",
            Price = 20000,
            Code = "1345",
            ProductType = ProductType.Service
        });
        _applicationDbContext.ProductServices.Add(new ProductService()
        {
            Name = "اسفنج نانو",
            Price = 30000,
            Code = "1445",
            ProductType = ProductType.Product
        });

        _applicationDbContext.Customers.Add(new Customer()
        {
            FirstName = "احمد",
            LastName = "زارع",
            Mobile = "09123456789"
        });

        _applicationDbContext.SaveChanges();
        return new EmptyResult();
    }

    public IActionResult NewInvoice(int id)
    {
       // var customer = _applicationDbContext.Customers.Find(id);
        var invoice=new Invoice()
        {
            CustomerId = id
        }
    }
}