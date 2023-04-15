using System.Security.Claims;
using Blog.Models;
using Blog.Services.EmailService;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class AuthController : Controller
{
    private readonly IEmailService _emailService;
    private readonly SignInManager<BlogUser> _signInManager;
    private readonly UserManager<BlogUser> _userManager;

    public AuthController(
        SignInManager<BlogUser> signInManager,
        UserManager<BlogUser> userManager,
        IEmailService emailService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _emailService = emailService;
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel vm)
    {
        if (!ModelState.IsValid) return RedirectToAction("SignIn");

        await _signInManager.PasswordSignInAsync(vm.EmailAddress, vm.Password, false, false);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = new BlogUser
        {
            UserName = vm.Email,
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            DateOfBirth = vm.DateOfBirth,
            Email = vm.Email,
            EmailConfirmed = false,
            PhoneNumber = vm.PhoneNumber,
            PhoneNumberConfirmed = false,
            Sex = vm.Sex.ToString()
        };
        var result = await _userManager.CreateAsync(user, vm.Password);

        if (!result.Succeeded)
        {
            return View(vm);
        }

        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, vm.FirstName));
        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, vm.Email));
        if (!result.Succeeded) return View(new RegisterViewModel());

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var link = Url.Action(nameof(EmailVerified), "Auth", new { userId = user.Id, token }, Request.Scheme,
            Request.Host.ToString());

        await _emailService.SendEmailAsync(vm.Email, "Your: Account",
            $"To verify your email follow the link below.\n{link}");

        return RedirectToAction("VerifyEmail", new { email = vm.Email });
    }

    public IActionResult VerifyEmail(string email)
    {
        return View(new VerifyEmailViewModel { Email = email });
    }

    public async Task<IActionResult> EmailVerified(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded) return View();

        return NotFound();
    }
}