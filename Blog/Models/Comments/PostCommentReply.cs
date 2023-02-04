#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Comments;

public class PostCommentReply
{
    public Guid PostCommentReplyId { get; set; }
    [Required] public string Message { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public Guid PostCommentId { get; set; }
    public PostComment PostComment { get; set; }
    public Guid BlogUserId { get; set; }
    public BlogUser BlogUser { get; set; }
}