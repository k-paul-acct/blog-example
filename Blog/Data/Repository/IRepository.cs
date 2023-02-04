using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Account;

namespace Blog.Data.Repository;

public interface IRepository
{
    bool AddPost(Post post);
    bool UpdatePost(PostViewModel post);
    bool RemovePost(Guid id);
    PostViewModel? GetPost(Guid id);

    PostsPageViewModel GetLatestPostsPage(string? category, string? searchQuery, int pageNumber, int limit,
        DateTime? cursorValue, bool afterCursor);

    void UpdateAccount(AccountViewModel vm);
    AccountViewModel? GetAccountData(Guid blogUserId);
    IEnumerable<CategoryViewModel> GetAllCategories();
    IEnumerable<TagViewModel> GetAllTags();
}