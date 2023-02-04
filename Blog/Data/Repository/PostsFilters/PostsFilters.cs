using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository.PostsFilters;

public static class PostsFilters
{
    public static IQueryable<Post> PostsWhereCategory(this IQueryable<Post> source, string category)
    {
        var normalizedCategory = category.ToUpper();
        return source.Where(x => x.PostCategory.NormalizedName == normalizedCategory);
    }

    public static IQueryable<Post> PostsWhereSearchQuery(this IQueryable<Post> source, string searchQuery)
    {
        return source.Where(x =>
            EF.Functions.Like(x.Title, $"%{searchQuery}%") || EF.Functions.Like(x.Body, $"%{searchQuery}%"));
    }

    public static IQueryable<Post> PostsWhereOlderThanCursorOrEqual(this IQueryable<Post> source, DateTime cursorValue)
    {
        return source.Where(x => x.Created <= cursorValue);
    }

    public static IQueryable<Post> PostsWhereNewerThanCursor(this IQueryable<Post> source, DateTime cursorValue)
    {
        return source.Where(x => x.Created > cursorValue);
    }
}