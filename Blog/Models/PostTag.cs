#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Blog.Models;

public class PostTag
{
    public Guid PostTagId { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string NormalizedName { get; set; } = string.Empty;

    public List<Post> Posts { get; set; }
}