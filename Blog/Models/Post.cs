namespace Blog.Models;

public class Post
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
}