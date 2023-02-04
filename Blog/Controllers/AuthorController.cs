using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class AuthorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}