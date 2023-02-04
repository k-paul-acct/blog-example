#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Comments;

public class PostComment
{
    public Guid PostCommentId { get; set; }
    [Required] public string Message { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public Guid PostId { get; set; }
    public Post Post { get; set; }
    public Guid BlogUserId { get; set; }
    public BlogUser BlogUser { get; set; }
    public List<PostCommentReply> PostCommentReplies { get; set; }
}