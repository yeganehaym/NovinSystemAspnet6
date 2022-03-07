using WebApplication2.Data;

namespace WebApplication2.Services;

public class ProductServices
{
    private ApplicationDbContext _context;

    public ProductServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddProductAsync(Data.Entity.ProductService productService)
    {
        await _context.ProductServices.AddAsync(productService);
    }
}