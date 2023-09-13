using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AuthApp.Models;

public class User : IdentityUser
{
    public string? profilePic { get; set; }

    public string? Bio { get; set; }
}