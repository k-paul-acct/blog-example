#nullable disable

namespace Blog.ViewModels;

public class PostViewModel
{
    public Guid PostId { get; set; }
    public BlogUserViewModel Author { get; set; }
    public string Category { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime Created { get; set; }
    public IFormFile Image { get; set; }
    public string ImageName { get; set; }
}