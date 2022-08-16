using Blog.Data.Repository;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize(Roles = "admin")]
public class PanelController : Controller
{
    private readonly IRepository _repository;

    public PanelController(IRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var posts = _repository.GetAllPosts();
        return View(posts);
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        var post = _repository.GetPost(id);
        return View(post);
    }

    [HttpPost]
    public IActionResult Edit(Post post)
    {
        _repository.UpdatePost(post);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Post());
    }

    [HttpPost]
    public IActionResult Create(Post post)
    {
        _repository.AddPost(post);
        return RedirectToAction("Post", "Home", new { id = post.PostId });
    }

    [HttpGet]
    public IActionResult Remove(Guid id)
    {
        var res = _repository.RemovePost(id);
        return RedirectToAction("Index");
    }
}