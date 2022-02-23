using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;

namespace WebApplication2.Models.Auth;

public class LoginPost
{
    [Required(ErrorMessage = "ورود شماره همراه الزامی است")]
    [ValidIranianMobileNumber(ErrorMessage = "شماره همراه معتبری را وارد نمایید")]
    public string Username { get; set; }
    [Required(ErrorMessage = "ورود کلمه عبور الزامی است")]
    [MinLength(6,ErrorMessage = "کلمه عبور حداقل 6 کارکتر باشد")]
    [MaxLength(15,ErrorMessage = "کلمه عبور نمی تواند بیش از 15 کارکتر باشد")]
    public string Password { get; set; }
    

}