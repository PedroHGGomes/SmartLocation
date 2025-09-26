using Microsoft.EntityFrameworkCore;

namespace SmartLocation.Api.Infrastructure;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize,
        string baseUrl)
    {
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PagedResult<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        result.Links["self"] = $"{baseUrl}?page={page}&pageSize={pageSize}";

        if (page > 1)
            result.Links["prev"] = $"{baseUrl}?page={page - 1}&pageSize={pageSize}";

        if (page * pageSize < totalCount)
            result.Links["next"] = $"{baseUrl}?page={page + 1}&pageSize={pageSize}";

        return result;
    }
}

