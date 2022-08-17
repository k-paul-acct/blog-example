namespace Blog.ViewModels;

public class PostViewModel
{
    public Guid PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public IFormFile? Image { get; set; } = null;
    public string ImageName { get; set; } = string.Empty;
}