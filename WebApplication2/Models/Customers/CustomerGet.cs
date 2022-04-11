namespace WebApplication2.Models.Customers;

public class CustomerGet
{
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    public int CustomerType { get; set; }
    public DateTime InsertTime { get; set; }
}