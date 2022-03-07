using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication2.Data;
using WebApplication2.Data.Domains;
using WebApplication2.Data.Entity;
using WebApplication2.Models.Products;

namespace WebApplication2.Controllers;

public class TestController : Controller
{
    private ApplicationDbContext _applicationDbContext;

    public TestController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public IActionResult Index()
    {
        SqlConnection connection = new SqlConnection("Server=.;Database=TestDb;User Id=sa;Password=123456789;");
        SqlCommand command = new SqlCommand("select * from products", connection);
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();

        var list = new List<ProductGet>();
        while (reader.Read())
        {
            var product = new ProductGet();
            product.Id = int.Parse(reader["id"].ToString());
            product.Name = reader["name"].ToString();
            product.Description = reader["description"].ToString();
            product.Price = int.Parse(reader["price"].ToString());
            product.Image = reader["image"].ToString();
            list.Add(product);
        }
        connection.Close();
        return View(list);
    }

    public IActionResult List()
    {
        var list = _applicationDbContext
            .Products
            .Where(p=>p.Price>200000 || p.CreationDate>DateTime.Now)
            .OrderByDescending(x=>x.Price)
            .ThenBy(x=>x.Name)
            .ToList();
        
        var count = _applicationDbContext
            .Products
            .Count(p => p.Price>200000 || p.CreationDate>DateTime.Now);
        var max = _applicationDbContext
            .Products
            .Max(x=>x.Price);
        
        var productList = new ProductList();
        productList.Products = list;
        productList.ProductCount = count;
        productList.MaxPrice = max;
        return View(productList);
    }

    public IActionResult GetCount()
    {
        var count = _applicationDbContext
            .Products
            .Count();
        return Content("Count=" + count);
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

        var invoice = new Invoice()
        {
            CustomerId = id,

        };
        var item1 = new InvoiceItem()
        {
            Invoice = invoice,
            ProductId = 1
        };
        var item2 = new InvoiceItem()
        {
            Invoice = invoice,
            ProductId = 2
        };
        var item3 = new InvoiceItem()
        {
            Invoice = invoice,
            ProductId =3
        };

        _applicationDbContext.Invoices.Add(invoice);
        _applicationDbContext.InvoiceItem.AddRange(item1,item2,item3);
        _applicationDbContext.SaveChanges();

        return new EmptyResult();
    }
}