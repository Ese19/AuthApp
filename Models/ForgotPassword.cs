using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models;

public class ForgotPassword
{
    [Required(ErrorMessage = "Please enter your email")]
    [EmailAddress]
    public string? Email { get; set; }
}