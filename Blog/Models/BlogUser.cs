#nullable disable

using System.ComponentModel.DataAnnotations;
using Blog.Models.Comments;
using Microsoft.AspNetCore.Identity;

namespace Blog.Models;

public class BlogUser : IdentityUser<Guid>
{
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string Sex { get; set; } = string.Empty;

    public List<Post> Posts { get; set; }
    public List<PostComment> PostComments { get; set; }
    public List<PostCommentReply> PostCommentReplies { get; set; }
}