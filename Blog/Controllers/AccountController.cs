using System.Security.Claims;
using Blog.Data.Repository;
using Blog.Models;
using Blog.Services.EmailService;
using Blog.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IEmailService _emailService;
    private readonly IRepository _repository;
    private readonly UserManager<BlogUser> _userManager;

    public AccountController(
        IRepository repository,
        UserManager<BlogUser> userManager,
        IEmailService emailService)
    {
        _repository = repository;
        _userManager = userManager;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        var blogUserId = Guid.Parse(_userManager.GetUserId(User));
        var vm = _repository.GetAccountData(blogUserId);
        return View(vm);
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult ChangePassword(ChangePasswordViewModel vm)
    {
        if (ModelState.IsValid) return BadRequest();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult ChangeEmail()
    {
        var currentEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;

        var vm = new ChangeEmailViewModel
        {
            NewEmail = currentEmail
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel vm)
    {
        if (!ModelState.IsValid) return BadRequest();

        var blogUser = await _userManager.GetUserAsync(User);

        var currentEmail = blogUser.Email;
        if (currentEmail == _userManager.NormalizeEmail(vm.NewEmail)) return RedirectToAction(nameof(Index));

        await _userManager.SetEmailAsync(blogUser, vm.NewEmail);
        await _userManager.SetUserNameAsync(blogUser, vm.NewEmail);
        var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
        await _userManager.ReplaceClaimAsync(blogUser, emailClaim, new Claim(ClaimTypes.Email, vm.NewEmail));

        var token = await _userManager.GenerateChangeEmailTokenAsync(blogUser, vm.NewEmail);
        var link = Url.Action(nameof(EmailChanged), "Account", new
            {
                blogUserId = blogUser.Id,
                token
            },
            Request.Scheme, Request.Host.ToString());

        await _emailService.SendEmailAsync(vm.NewEmail, "Your: Account",
            $"To confirm your new email follow the link below.\n{link}");

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EmailChanged(string blogUserId, string token)
    {
        var blogUser = await _userManager.FindByIdAsync(blogUserId);
        if (blogUser == null) return NotFound();

        var result = await _userManager.ChangeEmailAsync(blogUser, blogUser.Email, token);
        if (result.Succeeded) return View();

        return NotFound();
    }

    [HttpGet]
    public IActionResult ChangePersonalData()
    {
        var blogUserId = Guid.Parse(_userManager.GetUserId(User));
        var vm = _repository.GetAccountData(blogUserId);

        return View(vm);
    }

    [HttpPost]
    public IActionResult ChangePersonalData(AccountViewModel vm)
    {
        _repository.UpdateAccount(vm);
        return RedirectToAction(nameof(Index));
    }
}