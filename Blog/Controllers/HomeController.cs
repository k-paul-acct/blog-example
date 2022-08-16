using Blog.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class HomeController : Controller
{
    private readonly IRepository _repository;

    public HomeController(IRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var posts = _repository.GetAllPosts();
        return View(posts);
    }

    public IActionResult Post(Guid id)
    {
        var post = _repository.GetPost(id);
        if (post == null)
            return NotFound();
        return View(post);
    }
}