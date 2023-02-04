namespace Blog.ViewModels;

public class BlogUserViewModel
{
    public Guid BlogUserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}