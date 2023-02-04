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

    public IActionResult Index(
        string? category = null,
        string? searchQuery = null,
        int pageNumber = 1,
        int limit = 5,
        DateTime? cursorValue = null,
        bool afterCursor = true)
    {
        var page = _repository.GetLatestPostsPage(category, searchQuery, pageNumber, limit, cursorValue, afterCursor);
        return View(page);
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult AdminPanel()
    {
        return View();
    }

    [HttpGet("/image/{imageName}")]
    public IActionResult Image(string imageName)
    {
        var fileStream = _fileManager.GetFileStream(imageName);
        var extension = Path.GetExtension(imageName)[1..];
        return new FileStreamResult(fileStream, $"image/{extension}");
    }

    public IActionResult Contact()
    {
        return View();
    }
}