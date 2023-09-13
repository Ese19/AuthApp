using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuthApp.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AuthApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;

    private string _apiKey { get; set; }
    private string _apiSecret { get; set; }
    private string _cloud { get; set; }
    private Account _account { get; set; }

    private readonly IConfiguration configuration;

    public HomeController (ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IConfiguration configuration)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        this.configuration = configuration;
        _apiKey = configuration["Cloudinary:ApiKey"];
        _apiSecret = configuration["Cloudinary:ApiSecret"];
        _cloud = configuration["Cloudinary:ApiEnviromentVariable"];
        _account = new Account {ApiKey = _apiKey, ApiSecret = _apiSecret, Cloud = _cloud };

    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var email = this.User.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(email);
        return View(user);
    }


    [HttpGet]
    [Authorize]
    public IActionResult EditProfile()
    {
        return View();
    }


    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(AccountProfile model)
    {
        if(ModelState.IsValid)
        {
            var email = this.User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);



            if(model.Photo != null)
            {
                var fileName = model.Photo.FileName;
                string[] permittedExtensions = {".pdf", ".png", ".jpeg", ".svg", ".jpg", ".webp", ".jfif", ".pjpeg", ".pjpg"};
                var ext = Path.GetExtension(fileName);

                if(string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    ModelState.AddModelError(string.Empty, "File extension not permitted");
                    return View(model);
                }

                var cloudinary = new Cloudinary(_account);
                cloudinary.Api.Secure = true;

                byte[] bytes;

                using(var memoryStream = new MemoryStream())
                {
                    model.Photo.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                string base64 = Convert.ToBase64String(bytes);

                var prefix = @"data:image/png;base64,";
                var imagePath = prefix + base64;

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imagePath),
                    Folder = $"AuthApp/{user.Email}/"
                };

                var uploadResult = await cloudinary.UploadAsync(@uploadParams);

                if(user.profilePic != null)
                {
                    var deleteParams = new DeletionParams(user.profilePic);
                    await cloudinary.DestroyAsync(deleteParams);
                }

                user.profilePic = uploadResult.SecureUrl.AbsoluteUri;

            }

            if(model.Name != null)
            {
                user.UserName = model.Name;
            }


            user.Bio = model.Bio;

            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

        }
        
        return View();
        
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
