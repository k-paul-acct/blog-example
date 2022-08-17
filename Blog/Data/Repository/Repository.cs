using Blog.Models;
using Blog.ViewModels;

namespace Blog.Data.Repository;

public class Repository : IRepository
{
    private readonly BlogDbContext _context;

    public Repository(BlogDbContext context)
    {
        _context = context;
    }

    public bool AddPost(Post post)
    {
        _context.Add(post);
        return _context.SaveChanges() > 0;
    }

    public PostViewModel? GetPost(Guid id)
    {
        var post = _context.Posts.Find(id);
        if (post == null) return null;

        var postVm = new PostViewModel
        {
            PostId = post.PostId,
            Title = post.Title,
            Body = post.Body,
            Created = post.Created,
            ImageName = post.ImagePath
        };

        return postVm;
    }

    public IEnumerable<PostViewModel> GetAllPosts()
    {
        return _context.Posts.ToList().Select(x => new PostViewModel
        {
            PostId = x.PostId,
            Title = x.Title,
            Body = x.Body,
            Created = x.Created,
            ImageName = x.ImagePath
        });
    }

    public bool UpdatePost(PostViewModel postVm)
    {
        var post = _context.Posts.Find(postVm.PostId);
        if (post == null)
            return false;

        post.Title = postVm.Title;
        post.Body = postVm.Body;

        _context.Posts.Update(post);
        return _context.SaveChanges() > 0;
    }

    public bool RemovePost(Guid id)
    {
        var post = _context.Posts.Find(id);
        if (post == null)
            return false;
        _context.Remove(post);
        return _context.SaveChanges() > 0;
    }
}