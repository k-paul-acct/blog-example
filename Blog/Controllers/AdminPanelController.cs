using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize("Admin")]
public class AdminPanelController : Controller
{
    private readonly IFileManager _fileManager;
    private readonly IRepository _repository;

    public AdminPanelController(
        IRepository repository,
        IFileManager fileManager)
    {
        _repository = repository;
        _fileManager = fileManager;
    }


    public IActionResult Posts()
    {
        return NotFound();
    }

    public IActionResult Users()
    {
        throw new NotImplementedException();
    }

    public IActionResult Categories()
    {
        var categories = _repository.GetAllCategories();
        return View();
    }

    public IActionResult Tags()
    {
        var tags = _repository.GetAllTags();
        return View();
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
        return RedirectToAction("Posts");
    }

    [HttpGet]
    public IActionResult Remove(Guid id)
    {
        _repository.RemovePost(id);
        return RedirectToAction("Posts");
    }
}