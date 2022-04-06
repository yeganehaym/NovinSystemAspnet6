using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Data.Entity;

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

    public async Task RemoveProduct(int id)
    {
        var product =await _context.ProductServices.FindAsync(id);
        if(product==null)
            return;
        _context.ProductServices.Remove(product);
    }

    private IQueryable<ProductService> GetQuery(string search)
    {
        var query= _context.ProductServices
            .Where(x => x.ProductType == ProductType.Product);

        if (!string.IsNullOrEmpty(search))
        {
            query = query
                .Where(x => x.Name.Contains(search));
        }

        return query;
    }
    public async Task<List<ProductService>> GetProducts(int skip,int take,string search)
    {

        var query = GetQuery(search);
      
      return await query.OrderByDescending(x=>x.Id)
          .Skip(skip)
          .Take(take)
          .ToListAsync();

    }

    public async Task<int> GetProductsCount(string search)
    {
        var query = GetQuery(search);
        return await query
            .CountAsync();
    }
}