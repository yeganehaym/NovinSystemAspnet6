using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebApplication2.Data;
using WebApplication2.Models.Products;

namespace WebApplication2.Controllers;

public class ProductController : Controller
{
    private ApplicationDbContext _applicationDbContext;
    public ProductController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    // GET
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
}