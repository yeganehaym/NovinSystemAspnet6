namespace WebApplication2.Data.Entity;

/// <summary>
/// factor
/// </summary>
public class Invoice:BaseEntity
{
    public Customer Customer { get; set; }
    public int CustomerId { get; set; }
    
    public List<InvoiceItem> InvoiceItems { get; set; }
}