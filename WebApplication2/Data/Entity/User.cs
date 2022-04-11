using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Data.Entity;

public class User:BaseEntity
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string Username { get; set; }
    public string Password { get; set; }
    public string? Email { get; set; }
    public string MobileNumber { get; set; }
    
    public string? ImageProfile { get; set; }
    
    public bool IsAdmin { get; set; }
    public string SerialNo { get; set; }

    
    
    public UserStatus Status { get; set; }
    public List<OtpCode> OtpCodes { get; set; }
    public List<ProductService> Products { get; set; }
    public List<UserRole> UserRoles { get; set; }
    
}