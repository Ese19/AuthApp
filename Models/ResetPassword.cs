using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models;

public class ResetPassword
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    public string Email { get; set; }
    public string Token { get; set; }


}