#nullable disable

using System.ComponentModel.DataAnnotations;
using Blog.Models.Comments;
using Microsoft.EntityFrameworkCore;

namespace Blog.Models;

[Index(nameof(Created))]
public class Post
{
    public Guid PostId { get; set; }
    [Required] [MinLength(4)] public string Title { get; set; } = string.Empty;
    [Required] public string Body { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public uint Likes { get; set; } = 0;
    public uint Dislikes { get; set; } = 0;
    public uint Views { get; set; } = 0;
    public uint Favourites { get; set; } = 0;

    public Guid? PostCategoryId { get; set; }
    public PostCategory PostCategory { get; set; }
    public List<PostTag> PostTags { get; set; }
    public Guid? BlogUserId { get; set; }
    public BlogUser BlogUser { get; set; }
    public List<PostComment> PostComments { get; set; }
}