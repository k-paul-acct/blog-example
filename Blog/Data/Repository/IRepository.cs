using Blog.Models;

namespace Blog.Data.Repository;

public interface IRepository
{
    bool AddPost(Post post);
    Post? GetPost(Guid id);
    IEnumerable<Post> GetAllPosts();
    bool UpdatePost(Post post);
    bool RemovePost(Guid id);
}