using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize(Roles = "admin")]
public class PanelController : Controller
{
    private readonly IFileManager _fileManager;
    private readonly IRepository _repository;

    public PanelController(
        IRepository repository,
        IFileManager fileManager)
    {
        _repository = repository;
        _fileManager = fileManager;
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
    public IActionResult Edit(PostViewModel postVm)
    {
        _repository.UpdatePost(postVm);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new PostViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostViewModel postVm)
    {
        if (postVm.Image == null) return BadRequest();
        var imagePath = await _fileManager.SaveFile(postVm.Image);
        var post = new Post
        {
            PostId = postVm.PostId,
            Title = postVm.Title,
            Body = postVm.Body,
            ImagePath = imagePath
        };
        _repository.AddPost(post);
        return RedirectToAction("Post", "Home", new { id = post.PostId });
    }

    [HttpGet]
    public IActionResult Remove(Guid id)
    {
        _repository.RemovePost(id);
        return RedirectToAction("Index");
    }
}