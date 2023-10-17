using Ghak.libraries.AppBase.Models;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace Ghak.libraries.AppBase.Extensions;

public static class PaginationListExtension
{
      public static IEnumerable<IEnumerable<T>> ListsVariants<T>(this IEnumerable<IEnumerable<T>> sequences) {
        IEnumerable<IEnumerable<T>> result = new [] { Enumerable.Empty<T>() };
        foreach (var sequence in sequences) {
            var localSequence = sequence;
            result = result.SelectMany(
                _ => localSequence,
                (seq, item) => seq.Concat(new[] { item })
            );
        }
        return result;
    }
    
    public static IQueryable<T> OrderList<T>(this IQueryable<T> query,
        string? columnName = null,
        string? direction = null,
        string? orderableColumns = null)
    {
        var orderableColumnsList = (string.IsNullOrEmpty(orderableColumns))
            ? new List<string>()
            : orderableColumns
                .Replace(",", "|")
                .Split("|")
                .ToList();

        if (!orderableColumnsList.Contains("Id"))
            orderableColumnsList.Add("Id");
        
        if (!orderableColumnsList.Contains("CreatedAt"))
            orderableColumnsList.Add("CreatedAt");
        
        if (!orderableColumnsList.Contains("UpdatedAt"))
            orderableColumnsList.Add("UpdatedAt");
        
        
        if (string.IsNullOrEmpty(columnName) || orderableColumnsList.All(r => r != columnName))
        {
            columnName = "CreatedAt";
        }
        
        direction = string.IsNullOrEmpty(direction) ? "DESC" : direction.ToUpper();
        direction = (direction == "DESC") ? direction : "ASC";
        var q = columnName  + " " + direction;
        return query.OrderBy(q);
    }

    public static async Task<PaginationList<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        int page,
        int limit)
        where T : class
    {
        var paged = new PaginationList<T>();

        page = page < 0 ? 1 : page;

        paged.CurrentPage = page;
        paged.PageSize = limit;


        var startRow = (page - 1) * limit;

        paged.Items = await query
            .Skip(startRow)
            .Take(limit)
            .ToListAsync();

        paged.TotalItems = await query.CountAsync();
        paged.TotalPages = (int) Math.Ceiling(paged.TotalItems / (double) limit);


        return paged;
    }
}