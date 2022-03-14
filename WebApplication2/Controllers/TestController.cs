using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication2.Data;
using WebApplication2.Data.Domains;
using WebApplication2.Data.Entity;
using WebApplication2.Models.Products;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[Authorize]
public class TestController : Controller
{
    private ApplicationDbContext _applicationDbContext;
    private UserService _userService;
    private ProductServices _productServices;
    private IConfiguration _configuration;

    public TestController(ApplicationDbContext applicationDbContext,UserService userService, ProductServices productServices, IConfiguration configuration)
    {
        _applicationDbContext = applicationDbContext;
        _userService = userService;
        _productServices = productServices;
        _configuration = configuration;
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
        var userId = User.GetUserId();
   

        _applicationDbContext.ProductServices.Add(new ProductService()
        {
            Name = "شست و شو بدنه خودرو",
            Price = 10000,
            Code = "1245",
            ProductType = ProductType.Service,
            UserId = userId
        });
        
        _applicationDbContext.ProductServices.Add(new ProductService()
        {
            Name = "نظافت داخلی خودرو",
            Price = 20000,
            Code = "1345",
            ProductType = ProductType.Service,
            UserId = userId
        });
        _applicationDbContext.ProductServices.Add(new ProductService()
        {
            Name = "اسفنج نانو",
            Price = 30000,
            Code = "1445",
            ProductType = ProductType.Product,
            UserId = userId
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

    public async Task<IActionResult> TestUser()
    {
        var user = new User()
        {
            Username = "09323456789",
            MobileNumber = "09323456789",
            Password = "123456",
            FirstName = "ali",
            LastName = "yeganeh"
        };
        await _userService.AddUserAsync(user);

        await _productServices.AddProductAsync(new ProductService()
        {
            Name = "Test",
            Price = 400000,
        });
        await _applicationDbContext.SaveChangesAsync();
        return new EmptyResult();
    }

    public IActionResult Sms()
    {
        var apikey = _configuration["sms:ghasedak:apikey"];
        if (String.IsNullOrEmpty(apikey))
        {
            throw new Exception("SMS Api Key Is Empty");
        }
        return Content(apikey);
    }

    public IActionResult Cookie()
    {
        var cookie = Request.Cookies["testcookie"];
        return Content("Cookie Value:" + cookie);
    }
    
    public IActionResult WriteCookie()
    {
        Response.Cookies.Append("testcookie","this is a test cookie",new CookieOptions()
        {
            Expires = DateTimeOffset.Now.AddMinutes(1)
        });
        return Content("Cookie is Made");
    }

    [AllowAnonymous]
    public IActionResult TestString()
    {
        var x = new[] {"ali", "reza", "has,san"};
        var text = String.Join("-", x);
        var array = text.Split(new[] {'-', ','});
        return Content(text);
    }
}