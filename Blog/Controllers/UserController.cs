using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly UserManager<BlogUser> _userManager;

    public UserController(UserManager<BlogUser> userManager)
    {
        _userManager = userManager;
    }
}