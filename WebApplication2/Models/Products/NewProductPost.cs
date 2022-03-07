using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.Products;

public class NewProductPost
{
    [MaxLength(10,ErrorMessage = "کد محصول بیشتر از ده کاراکتر نمی تواند باشد")]
    public string? Code { get; set; }
    
    [Required(ErrorMessage = "ورود نام محصول الزامی است")]
    [MaxLength(100,ErrorMessage = "کد محصول بیشتر از صد کاراکتر نمی تواند باشد")]
    public string Name { get; set; }
    public int Price { get; set; }
}