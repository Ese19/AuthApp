using AuthApp.EmailService;
using AuthApp.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Controllers;


public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;

    private readonly IEmailSender _emailSender;



    public AuthController (ILogger<AuthController> logger, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IEmailSender emailSender)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _emailSender = emailSender;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Register model)
    {
        if(ModelState.IsValid)
        {
            // Todo
            var user = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(user, model.Password);


            if(result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.Action("ConfirmEmail", "Auth", 
                                        new { email = user.Email, token = token }, Request.Scheme);

                var message = new Message(new string[] {user.Email}, "Confimation email link", confirmationLink); 

                await _emailSender.SendEmailAsync(message);

                
                return RedirectToAction(nameof(SuccessRegistration));
            }

        }
        return View(model);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Login model)
    {
        if(ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                return View(model);
            }

            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
    
                ModelState.AddModelError("", "Invalid name or password");
            }
            
            
        }

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if(user == null)
        {
            return View("Error");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if(result.Succeeded)
        {
            return View("ConfirmEmail");
        }

        return View("Error");
    }

    [HttpGet]
    public IActionResult SuccessRegistration()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        // todo
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Auth");
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
    {
        if(!ModelState.IsValid)
            return View(forgotPassword);

        var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
        if(user == null)
        {
            return  RedirectToAction("ForgotPasswordConfirmation", "Auth");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);                                                                                   
        var callback = Url.Action("ResetPassword", "Auth", new { token , email = user.Email}, Request.Scheme);
        
        var message = new Message(new string[] {user.Email}, "Reset password token", callback);
        await _emailSender.SendEmailAsync(message);

        return RedirectToAction("ForgotPasswordConfirmation", "Auth");

    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        var model = new ResetPassword  { Email = email, Token = token};

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]                                                                                        
    public async Task<IActionResult> ResetPassword(ResetPassword model)
    {
        if(!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if(user == null)
        {
            return RedirectToAction("ResetPasswordConfirmation", "Auth");
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if(!result.Succeeded)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }   

        return RedirectToAction("ResetPasswordConfirmation", "Auth");             
    }

    [HttpGet]
    public IActionResult ResetPasswordconfirmation()
    {
        return View();
    }



    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }
}

    