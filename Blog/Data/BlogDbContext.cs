#nullable disable

using Blog.Models;
using Blog.Models.Comments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data;

public class BlogDbContext : IdentityDbContext<BlogUser, IdentityRole<Guid>, Guid>
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<PostCategory> PostCategory { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<PostCommentReply> PostCommentReplies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PostComment>()
            .HasOne(e => e.BlogUser)
            .WithMany(e => e.PostComments)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PostCommentReply>()
            .HasOne(e => e.BlogUser)
            .WithMany(e => e.PostCommentReplies)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<BlogUser>()
            .HasMany(x => x.Posts)
            .WithOne(x => x.BlogUser)
            .OnDelete(DeleteBehavior.SetNull);
    }
}