#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Blog.Models;

public class PostCategory
{
    public Guid PostCategoryId { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string NormalizedName { get; set; } = string.Empty;

    public List<Post> Posts { get; set; }
}