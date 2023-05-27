using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize]
public class PostController : Controller
{
    private readonly IFileManager _fileManager;
    private readonly IRepository _repository;
    private readonly UserManager<BlogUser> _userManager;

    public PostController(IFileManager fileManager, IRepository repository, UserManager<BlogUser> userManager)
    {
        _fileManager = fileManager;
        _repository = repository;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [Route("[Controller]")]
    public IActionResult Index(Guid postId)
    {
        var vm = _repository.GetPost(postId);
        if (vm == null) return NotFound();

        return View(vm);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new PostViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostViewModel vm)
    {
        if (vm.Image == null) return BadRequest();
        var imagePath = await _fileManager.SaveFile(vm.Image);
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user is null) return new BadRequestResult();

        var blogUserId = user.Id;
        var post = new Post
        {
            PostId = vm.PostId,
            Title = vm.Title,
            Body = vm.Body,
            ImageName = imagePath,
            BlogUserId = blogUserId,
            PostCategoryId = new Guid("ae5acdf8-9da5-4e98-862f-0ca8f8d57f70")
        };
        _repository.AddPost(post);
        return RedirectToAction("Index", new { postId = post.PostId });
    }

    public IActionResult Delete(Guid postId)
    {
        _repository.RemovePost(postId);   
        return RedirectToAction("Index", "Home");
    }
}