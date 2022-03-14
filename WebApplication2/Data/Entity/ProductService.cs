namespace WebApplication2.Data.Entity;

public class ProductService:BaseEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public ProductType ProductType { get; set; }
    
    public List<InvoiceItem> InvoiceItems { get; set; }
    
    public User User { get; set; }
    public int UserId { get; set; }
}