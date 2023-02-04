#nullable disable

namespace Blog.ViewModels;

public class PostsPageViewModel
{
    public string Category { get; set; }
    public string SearchQuery { get; set; }
    public int PageNumber { get; set; }
    public int Limit { get; set; }
    public DateTime? CursorValueStart { get; set; }
    public DateTime? CursorValueEnd { get; set; }
    public IEnumerable<PostViewModel> Posts { get; set; }
}