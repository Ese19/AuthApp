using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models;

public class AccountProfile
{
    public IFormFile? Photo { get; set; }
    public string? Name { get; set; }

    public string? Bio { get; set; }

}