using Blog.Models;

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

    public Post? GetPost(Guid id)
    {
        return _context.Posts.Find(id);
    }

    public IEnumerable<Post> GetAllPosts()
    {
        return _context.Posts.ToList();
    }

    public bool UpdatePost(Post post)
    {
        _context.Posts.Update(post);
        return _context.SaveChanges() > 0;
    }

    public bool RemovePost(Guid id)
    {
        var post = GetPost(id);
        if (post == null)
            return false;
        _context.Remove(post);
        return _context.SaveChanges() > 0;
    }
}