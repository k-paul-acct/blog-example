using Blog.Data.FileManager;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class HomeController : Controller
{
    private readonly IFileManager _fileManager;
    private readonly IRepository _repository;

    public HomeController(
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

    public IActionResult Post(Guid id)
    {
        var post = _repository.GetPost(id);
        if (post == null)
            return NotFound();
        return View(post);
    }

    [HttpGet("/image/{imageName}")]
    public IActionResult Image(string imageName)
    {
        var fileStream = _fileManager.GetFileStream(imageName);
        var extension = Path.GetExtension(imageName)[1..];
        return new FileStreamResult(fileStream, $"image/{extension}");
    }
}