using WebApplication2.Data.Domains;

namespace WebApplication2.Models.Products;

public class ProductList
{
    public  List<Product> Products { get; set; }
    public int ProductCount { get; set; }
    public int MaxPrice { get; set; }
}