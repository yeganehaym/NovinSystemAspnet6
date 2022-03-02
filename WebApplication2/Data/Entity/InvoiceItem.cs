namespace WebApplication2.Data.Entity;

public class InvoiceItem
{
    public Invoice Invoice { get; set; }
    public int InvoiceId { get; set; }
    public ProductService Product { get; set; }
    public int ProductId { get; set; }
    
    public int Quantity { get; set; }
}