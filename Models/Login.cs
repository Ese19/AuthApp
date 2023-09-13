using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models;

public class Login
{
    [Required(ErrorMessage = "Please enter your email")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}