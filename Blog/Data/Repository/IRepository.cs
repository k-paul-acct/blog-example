using Blog.Models;
using Blog.ViewModels;

namespace Blog.Data.Repository;

public interface IRepository
{
    bool AddPost(Post post);
    PostViewModel? GetPost(Guid id);
    IEnumerable<PostViewModel> GetAllPosts();
    bool UpdatePost(PostViewModel post);
    bool RemovePost(Guid id);
}