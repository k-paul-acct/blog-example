using System.Security.Claims;
using Blog.Data.Repository.PostsFilters;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Account;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository;

public class Repository : IRepository
{
    private readonly BlogDbContext _context;
    private readonly IHttpContextAccessor _httpContext;

    public Repository(BlogDbContext context, IHttpContextAccessor httpContext)
    {
        _context = context;
        _httpContext = httpContext;
    }

    public bool AddPost(Post post)
    {
        _context.Add(post);
        return _context.SaveChanges() > 0;
    }

    public bool UpdatePost(PostViewModel postVm)
    {
        var post = _context.Posts.Find(postVm.PostId);
        if (post == null) return false;

        post.Title = postVm.Title;
        post.Body = postVm.Body;

        _context.Posts.Update(post);
        return _context.SaveChanges() > 0;
    }

    public bool RemovePost(Guid id)
    {
        var post = _context.Posts.Find(id);
        if (post == null) return false;

        _context.Remove(post);
        return _context.SaveChanges() > 0;
    }

    public PostViewModel? GetPost(Guid id)
    {
        var post = _context.Posts
            .Include(x => x.BlogUser)
            .Include(x => x.PostCategory)
            .SingleOrDefault(x => x.PostId == id);

        return post == null ? null : MapPost(post);
    }

    public PostsPageViewModel GetLatestPostsPage(string? category, string? searchQuery, int pageNumber, int limit,
        DateTime? cursorValue, bool afterCursor)
    {
        var query = _context.Posts.AsQueryable();

        if (cursorValue != null)
            query = afterCursor
                ? query.PostsWhereOlderThanCursorOrEqual(cursorValue.Value)
                : query.PostsWhereNewerThanCursor(cursorValue.Value);

        if (!string.IsNullOrEmpty(category)) query = query.PostsWhereCategory(category);

        if (!string.IsNullOrEmpty(searchQuery)) query = query.PostsWhereSearchQuery(searchQuery);

        query = afterCursor
            ? query.OrderByDescending(x => x.Created)
            : query.OrderBy(x => x.Created).Skip(1);

        if (cursorValue == null) query = query.Skip((pageNumber - 1) * limit);

        var posts = query
            .Take(limit)
            .OrderByDescending(x => x.Created)
            .Include(x => x.BlogUser)
            .Include(x => x.PostCategory)
            .ToList();
        
        var mapped = posts.Select(MapPost);

        return new PostsPageViewModel
        {
            Category = category,
            SearchQuery = searchQuery,
            PageNumber = pageNumber,
            Limit = limit,
            CursorValueStart = posts.Count == 0 ? null : posts.First().Created,
            CursorValueEnd = posts.Count == 0 ? null : posts.Last().Created,
            Posts = mapped
        };
    }

    public void UpdateAccount(AccountViewModel vm)
    {
        var blogUser = _context.Users.Find(vm.BlogUserId);

        if (blogUser == null) return;

        blogUser.About = vm.About;
        blogUser.Sex = vm.Sex;
        blogUser.FirstName = vm.FirstName;
        blogUser.LastName = vm.LastName;
        blogUser.DateOfBirth = vm.DateOfBirth;

        _context.Users.Update(blogUser);
        _context.SaveChanges();
    }

    public AccountViewModel? GetAccountData(Guid blogUserId)
    {
        var blogUser = _context.Users.Find(blogUserId);
        return blogUser == null ? null : MapAccount(blogUser);
    }

    public IEnumerable<CategoryViewModel> GetAllCategories()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TagViewModel> GetAllTags()
    {
        throw new NotImplementedException();
    }

    private PostViewModel MapPost(Post post)
    {
        var isAdmin = _httpContext.HttpContext!.User.Claims.Any(x => x is { Value: "admin" });
        return new PostViewModel
        {
            PostId = post.PostId,
            Author = MapBlogUser(post.BlogUser, isAdmin),
            Category = post.PostCategory.Name,
            Title = post.Title,
            Body = post.Body,
            Created = post.Created,
            ImageName = post.ImageName
        };
    }

    private static BlogUserViewModel MapBlogUser(BlogUser blogUser, bool isAdmin)
    {
        return new BlogUserViewModel
        {
            FirstName = blogUser.FirstName,
            LastName = blogUser.LastName,
            BlogUserId = blogUser.Id,
            IsAdmin = isAdmin
        };
    }

    private static AccountViewModel MapAccount(BlogUser blogUser)
    {
        return new AccountViewModel
        {
            FirstName = blogUser.FirstName,
            LastName = blogUser.LastName,
            Email = blogUser.Email,
            About = blogUser.About,
            DateOfBirth = blogUser.DateOfBirth,
            Sex = blogUser.Sex
        };
    }
}