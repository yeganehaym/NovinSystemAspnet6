namespace WebApplication2.Data.Entity;

public class UserRole
{
    public Role Role { get; set; }
    public int RoleId { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
}