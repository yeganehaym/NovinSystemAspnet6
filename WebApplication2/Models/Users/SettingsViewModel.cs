namespace WebApplication2.Models.Users;

public class SettingsViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IFormFile Avatar { get; set; }
}