namespace WebApplication2.Data.Entity;

public class Customer:BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    
    public List<Invoice> Invoices { get; set; }
}