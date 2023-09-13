using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models;

public class Register
{
    [Required(ErrorMessage = "Please enter your name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please enter your email")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}