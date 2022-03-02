using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.Auth;

public class OtpCodePost
{
    [Required]
    [MaxLength(6)]
    public string Code { get; set; }
}